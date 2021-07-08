using UnityEditor;
using UnityEngine;

namespace ThroughAThousandEyes.CheatsModule
{
    public class CheatsWindow : EditorWindow
    {
        private long _addSilkAmount;
        
        [MenuItem("Window/Cheats")]
        private static void Open()
        {
            CheatsWindow window = EditorWindow.GetWindow<CheatsWindow>("Cheats");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Add silk");
            EditorGUILayout.BeginHorizontal();
            _addSilkAmount = EditorGUILayout.LongField(_addSilkAmount);
            bool addSilk = GUILayout.Button("Add");
            EditorGUILayout.EndHorizontal();

            if (addSilk)
            {
                CheatsModuleFacade.Instance.MainModuleFacade.Inventory.Silk += _addSilkAmount;
            }
        }
    }
}