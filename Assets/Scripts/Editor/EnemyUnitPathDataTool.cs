using System;
using System.Collections.Generic;
using GeoDefence.Map;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EnemyUnitPathDataTool : EditorWindow
    {
        /// <summary>
        /// List of cell space positions for the path.
        /// </summary>
        private List<Vector3Int> positions;

        private MapManager m_mapManager;
        private MapManager MapManager
        {
            get
            {
                if (m_mapManager == null)
                    m_mapManager = FindObjectOfType<MapManager>();
                
                m_mapManager?.GeneratePropertyMap();
                return m_mapManager;
            }
        }

        private bool editing;
        
        [MenuItem("GeoDefence/Path Data Tool")]
        static void Init()
        {
            var window = GetWindow<EnemyUnitPathDataTool>(true, "Path Tool");

            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;

            // Load positions already in the path data list if they exist.
            positions = new List<Vector3Int>();
            foreach (var pos in MapManager.PathData.CellPositions)
            {
                positions.Add(pos);
            }
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            if (MapManager == null || MapManager.PathData == null)
            {
                GUILayout.Label("No Map Manager is present in the scene, or the path data has not been assigned.", 
                    EditorStyles.label);
                return;
            }

            var defaultColor = GUI.color;

            GUILayout.BeginHorizontal();
            {
                GUI.color = Color.white;
                GUILayout.Label("Path Data Tool", EditorStyles.whiteLargeLabel);
                GUI.color = defaultColor;

                GUI.color = Color.yellow;
                if (GUILayout.Button("Rebuild map data"))
                {
                    MapManager.GeneratePropertyMap(true);
                }
                GUI.color = defaultColor;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(2);
            GUILayout.Label("Position count: " + positions.Count);
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            {
                GUI.color = editing ? Color.yellow : Color.green;
                if (GUILayout.Button(!editing ? "Start Edit" : "End Edit"))
                {
                    editing = !editing;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Clear path"))
                {
                    positions.Clear();
                    MapManager.PathData.CellPositions?.Clear();
                }
            }
            GUILayout.EndHorizontal();

            if (!IsPathValid())
            {
                GUI.color = Color.white;
                EditorGUILayout.HelpBox("Path is not valid.", MessageType.Error);
            }
            else
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Submit path"))
                {
                    MapManager.PathData.SetPosition(positions);
                }
            }
            
            GUI.color = defaultColor;
            
            if (editing)
            {
                EditorGUILayout.HelpBox(IsPathValid() ? "Path is complete." : "Click tile map to add points to path.",
                    MessageType.Info);
            }
        }

        private void OnSceneGUI(SceneView _sceneView)
        {
            if (editing)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                var mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                
                Handles.color = new Color(1, 0, 0, 0.35f);
                if (IsPositionValid(mousePos))
                {
                    Handles.color = new Color(0, 1, 0, 0.35f);
                    if (Event.current.type == EventType.MouseDown)
                    {
                        positions.Add(MapManager.WorldToCell(mousePos));
                        if (MapManager.GetTileProperty(mousePos)?.Type == TileType.End)
                            editing = false;
                    }
                }
                
                Handles.DrawWireCube(MapManager.CellToWorld(MapManager.WorldToCell(mousePos)) + Vector3.one * 0.5f,
                    Vector3.one);
            }

            for (var i = 0; i < positions.Count - 1; i++)
            {
                var currentPos = MapManager.CellToWorld(positions[i]) + Vector3.one * 0.5f;
                var nextPos = MapManager.CellToWorld(positions[i + 1]) + Vector3.one * 0.5f;
                
                Handles.color = Color.white;
                Handles.DrawLine(currentPos, nextPos);
            }

            for (var i = 0; i < positions.Count; i++)
            {
                var worldPos = MapManager.CellToWorld(positions[i]) + Vector3.one * 0.5f;
                
                if (i == 0 || i == positions.Count - 1)
                {
                    Handles.color = Color.blue;
                    Handles.DrawSolidDisc(worldPos, Vector3.forward, 0.2f);
                    continue;
                }
                
                Handles.color = Color.white;
                Handles.DrawSolidDisc(worldPos, Vector3.forward, 0.1f);
            }
            
            HandleUtility.Repaint();
            Repaint();
        }

        private bool IsPositionValid(Vector3 _worldPos)
        {
            // Check if tile is already in list.
            if (positions.Contains(MapManager.WorldToCell(_worldPos))) return false;

            var tile = MapManager.GetTileProperty(_worldPos)?.Type;

            // First tile must be a spawn tile.
            if (positions.Count == 0) 
                return tile == TileType.Spawn;
            
            // Check if the last tile in the list is already the end tile.
            if (MapManager.GetTileProperty(positions[positions.Count - 1])?.Type == TileType.End) return false;
            
            // Only path and end tile types can be added to the path list after the spawn tile.
            if (positions.Count > 0) 
                return tile == TileType.Path || tile == TileType.End;
            
            return false;
        }

        private bool IsPathValid()
        {
            // Path needs at least 3 points. (spawn -> path -> end)
            if (positions.Count < 3) return false;

            var start = MapManager.GetTileProperty(positions[0])?.Type == TileType.Spawn;
            var end = MapManager.GetTileProperty(positions[positions.Count - 1])?.Type == TileType.End;
            return start && end;
        }
    }
}