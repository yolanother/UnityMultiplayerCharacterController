using UnityEngine;

namespace DoubTech.MCC.CharacterSelection
{
    public class Armature : MonoBehaviour
    {
        [SerializeField] public MaterialSet thirdPerson;
        [SerializeField] public MaterialSet firstPerson;

        private bool fpsMode;
        
        public int SelectedMaterial
        {
            get => fpsMode ? firstPerson.SelectedMaterial : thirdPerson.SelectedMaterial;
            set
            {
                firstPerson.SelectedMaterial = value;
                thirdPerson.SelectedMaterial = value;
            }
        }
        
        public bool FPSMode
        {
            get => fpsMode;
            set
            {
                fpsMode = value;
                firstPerson.gameObject.SetActive(fpsMode);
                thirdPerson.gameObject.SetActive(!fpsMode);
                firstPerson.FPSMode = value;
                thirdPerson.FPSMode = value;
            }
        }

        public int MaterialCount => fpsMode ? firstPerson.Count : thirdPerson.Count;
    }
}