using UnityEngine;

namespace DoubTech.MCC.CharacterSelection
{
    public class MaterialSet : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private bool fpsMode;

        [Header("Variant")]
        [SerializeField] protected int selectedMaterial;

        [Header("Components")]
        [SerializeField] private Material[] materials;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private GameObject[] fpsDisabledObjects;

        public virtual int Count => materials.Length;

        private void OnValidate()
        {
            SelectedMaterial = selectedMaterial;
            FPSMode = fpsMode;
        }

        private void OnEnable()
        {
            SelectedMaterial = selectedMaterial;
            FPSMode = fpsMode;
        }

        public virtual int SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                if (value < materials.Length)
                {
                    selectedMaterial = value;
                    foreach (var r in renderers)
                    {
                        r.material = materials[selectedMaterial];
                    }
                }
            }
        }

        public bool FPSMode
        {
            get => fpsMode;
            set
            {
                fpsMode = value;
                foreach (var disabledObject in fpsDisabledObjects)
                {
                    disabledObject.SetActive(!fpsMode);
                }
            }
        }
    }
}
