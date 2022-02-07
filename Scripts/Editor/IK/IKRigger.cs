using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DoubTech.MCC.IK
{
    public class IKRigger : EditorWindow
    {
        [SerializeField] private GameObject characterRigPrefab;

        [SerializeField] private Animator armatureRoot;
        [SerializeField] private Transform aimTarget;

        [MenuItem("Tools/DoubTech/MCC/IKRigger")]
        static void Init()
        {
            IKRigger window = EditorWindow.GetWindow(typeof(IKRigger)) as IKRigger;
            window.titleContent = new GUIContent("IKRigger");
            window.autoRepaintOnSceneChange = true;
            window.Show();
        }

        private void OnSelectionChange()
        {
            var animator = Selection.activeGameObject?.GetComponent<Animator>();
            if (animator) armatureRoot = animator;
        }

        protected virtual void OnGUI()
        {
            armatureRoot =
                EditorGUILayout.ObjectField("Active Armature", armatureRoot, typeof(Animator)) as
                    Animator;
            characterRigPrefab =
                EditorGUILayout.ObjectField("Rig Prefab", characterRigPrefab, typeof(GameObject)) as
                    GameObject;
            aimTarget =
                EditorGUILayout.ObjectField("Aim Target", aimTarget, typeof(Transform)) as
                    Transform;

            if (!armatureRoot)
            {
                GUILayout.Label("Select a game object with an animator to get started.");
                return;
            }

            if (GUILayout.Button("Rig"))
            {
                var rig = armatureRoot.transform.Find(characterRigPrefab.name)?.gameObject;
                if (!rig) rig = Instantiate(characterRigPrefab, armatureRoot.transform);
                rig.name = characterRigPrefab.name;

                var rigBuilder = armatureRoot.GetComponent<RigBuilder>();
                if (!rigBuilder)
                {
                    armatureRoot.gameObject.AddComponent<RigBuilder>();
                    var layer = new RigLayer(rig.GetComponent<Rig>());
                    rigBuilder.layers.Add(layer);
                    EditorUtility.SetDirty(rigBuilder);
                }

                EditorUtility.SetDirty(rig);

                var boneAssignments = rig.GetComponentsInChildren<BoneAssignment>();
                foreach (var boneAssignment in boneAssignments)
                {
                    var bone = armatureRoot.GetBoneTransform(boneAssignment.assignedBone);
                    var multiaim = boneAssignment.GetComponent<MultiAimConstraint>();
                    if (multiaim)
                    {
                        multiaim.data.constrainedObject = bone;
                        if (multiaim.data.sourceObjects.Count > 0)
                        {
                            multiaim.data.sourceObjects.SetTransform(0, aimTarget);
                        }

                        EditorUtility.SetDirty(multiaim);
                    }
                }
            }
        }
    }
}
