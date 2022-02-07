using System;
using UnityEngine;

namespace DoubTech.MCC.CharacterSelection
{
    public class AdvancedMaterialSet : MaterialSet
    {
        [Header("Components")]
        [SerializeField] private AssignedMaterials[] advancedMaterials;

        public override int Count => advancedMaterials.Length;

        public override int SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                foreach (var assignedMaterial in advancedMaterials)
                {
                    var materials = assignedMaterial.materials;
                    if (value < materials.Length)
                    {
                        selectedMaterial = value;
                        foreach (var r in assignedMaterial.renderers)
                        {
                            r.material = materials[selectedMaterial];
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public class AssignedMaterials
    {
        public Renderer[] renderers;
        public Material[] materials;
    }
}
