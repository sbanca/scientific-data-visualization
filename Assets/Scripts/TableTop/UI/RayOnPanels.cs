using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnPanels : Singleton<RayOnPanels>
    {

        private RaycastHit hit;

        private Ray ray;

        private Camera maincam;

        private int PanelsLayer;

        private void Start()
        {

            // Bit shift the index of the layer(8) to get a bit mask

            PanelsLayer = 1 << LayerMask.NameToLayer("Panels");

            maincam = Camera.main;

        }

        public RaycastHit? PanelHit()
        {

           
            ray = maincam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, PanelsLayer))
            {
                return hit;
            }

            else
            {
                return null;
            }

            
        }
    }
}
