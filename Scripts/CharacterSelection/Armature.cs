using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DoubTech.MCC.CharacterSelection
{
    public class Armature : MonoBehaviour
    {
        [SerializeField] public MaterialSet thirdPerson;
        [SerializeField] public MaterialSet firstPerson;

        private bool fpsMode;
        private bool initialized;

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
                if(initialized) ResetRigBuilder();
            }
        }

        private void OnEnable()
        {
            ResetRigBuilder();
            initialized = true;
        }

        private void OnDisable()
        {
            initialized = false;
        }

        public MaterialSet ActiveMaterialSet => fpsMode ? firstPerson : thirdPerson;

        public void ResetRigBuilder() => StartCoroutine(ResetRigBuilder(ActiveMaterialSet));
        
        private IEnumerator ResetRigBuilder(MaterialSet materialSet)
        {
            var rigBuilder = materialSet.GetComponent<RigBuilder>();
            // Hacky solution to reset Animation rigging and make sure it is working
            yield return new WaitForSeconds(.1f);
            yield return new WaitForEndOfFrame();
            rigBuilder.enabled = false;
            yield return new WaitForSeconds(2f);
            yield return new WaitForEndOfFrame();
            rigBuilder.enabled = true;
        }

        public int MaterialCount => fpsMode ? firstPerson.Count : thirdPerson.Count;
    }
}