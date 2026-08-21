using UnityEditor;
using UnityEngine;

namespace KuzuStudios.KFSM.Editor
{
    public class KFSMControllerAssetDeletionProcessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            KFSMController targetKFSMController = AssetDatabase.LoadAssetAtPath<KFSMController>(assetPath);

            if (targetKFSMController != null)
            {
                KFSMEditorWindow.Controller = null;
            }
            
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
