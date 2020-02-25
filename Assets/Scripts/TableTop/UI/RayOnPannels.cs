using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop
{
    public class RayOnPannels : Singleton<RayOnPannels>
    {

        private RaycastHit hit;

        private Ray ray;

        private Camera maincam;

        private int PannelsLayer;

        private void Start()
        {

            // Bit shift the index of the layer(8) to get a bit mask

            PannelsLayer = 1 << LayerMask.NameToLayer("Pannels");

            maincam = Camera.main;

        }

        public RaycastHit? PannelHit()
        {

           
            ray = maincam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, PannelsLayer))
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
