//using DotEditor.Core.EGUI;
//using DotEditor.EGUI.Window;
//using UnityEditor;
//using UnityEngine;
//using UnityObject = UnityEngine.Object;

//namespace DotEditor.Asset.AssetPacker
//{
//    public class AssetAddressRepeatPopupWindow : PopupWindow
//    {
//        private static int WIN_WIDTH = 600;
//        private static int WIN_Height = 300;

//        private Vector2 scrollPos = Vector2.zero;
//        private AssetPackerAddressData[] repeatAddressDatas;

//        public static void ShowWin(AssetPackerAddressData[] datas, Vector2 position)
//        {
//            var win = ShowPopupWin<AssetAddressRepeatPopupWindow>(new Rect(position + new Vector2(10, 20), new Vector2(WIN_WIDTH, WIN_Height)),false);
//            win.repeatAddressDatas = datas;
//        }

//        protected override void DrawElement()
//        {
//            GUIStyle boldCenterStyle = new GUIStyle(EditorStyles.label);
//            boldCenterStyle.alignment = TextAnchor.MiddleCenter;
//            boldCenterStyle.fontStyle = FontStyle.Bold;

//            if(repeatAddressDatas!=null && repeatAddressDatas.Length>0)
//            {
//                EditorGUILayout.LabelField($"Repeat Address({repeatAddressDatas[0].assetAddress})", boldCenterStyle);
//                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
//                {
//                    int index = 0;
//                    foreach (var data in repeatAddressDatas)
//                    {
//                        EditorGUILayout.BeginHorizontal();
//                        {
//                            EditorGUILayout.LabelField("" + index, GUILayout.Width(20));
//                            DotEditorGUI.BeginLabelWidth(60);
//                            {
//                                EditorGUILayout.TextField("Path : ", data.assetPath);
//                            }
//                            DotEditorGUI.EndLableWidth();
//                            UnityObject uObj = AssetDatabase.LoadAssetAtPath<UnityObject>(data.assetPath);
//                            EditorGUILayout.ObjectField(uObj, typeof(UnityObject), true, GUILayout.Width(160));
//                        }
//                        EditorGUILayout.EndHorizontal();

//                        ++index;
//                    }
//                }
//                EditorGUILayout.EndScrollView();
//            }
            
//        }
//    }
//}
