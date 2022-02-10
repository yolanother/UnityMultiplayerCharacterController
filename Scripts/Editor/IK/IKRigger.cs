using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DoubTech.MCC.IK
{
    public class IKRigger : EditorWindow
    {
        [SerializeField] private GameObject characterRigPrefab;
        [SerializeField] private Animator armatureRoot;

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
            
            if (!armatureRoot)
            {
                GUILayout.Label("Select a game object with an animator to get started.");
                return;
            }

            if (GUILayout.Button("Rig"))
            {
                var rig = armatureRoot.transform.Find(characterRigPrefab.name)?.gameObject;
                if (!rig) rig = (GameObject) PrefabUtility.InstantiatePrefab(characterRigPrefab);
                rig.transform.parent = armatureRoot.transform;
                rig.transform.localPosition = Vector3.zero;

                rig.name = characterRigPrefab.name;

                
                var rigBuilder = armatureRoot.GetComponent<RigBuilder>();
                if (!rigBuilder)
                {
                    rigBuilder = armatureRoot.gameObject.AddComponent<RigBuilder>();
                }
                
                var layer = new RigLayer(rig.GetComponent<Rig>());
                if (rigBuilder.layers.Count == 1)
                {
                    rigBuilder.layers[0] = layer;
                }
                else
                {
                    rigBuilder.layers.Add(layer);
                }
                EditorUtility.SetDirty(rigBuilder);

                EditorUtility.SetDirty(rig);

                IKTarget aimTarget = armatureRoot.GetComponentsInChildren<IKTarget>().FirstOrDefault(a => a.IKTargetType == IKTargetType.Aim);
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
                            var array = new WeightedTransformArray();
                            array.Add(new WeightedTransform(aimTarget.transform, 1));
                            multiaim.data.sourceObjects = array;
                        }

                        EditorUtility.SetDirty(multiaim);
                    }

                    var twoBone = boneAssignment.GetComponent<TwoBoneIKConstraint>();
                    if (twoBone)
                    {
                        twoBone.data.tip = bone;
                        twoBone.data.mid = bone.parent;
                        twoBone.data.root = bone.parent.parent;
                        EditorUtility.SetDirty(twoBone);
                    }
                }
            }
            
        }
    }
}
