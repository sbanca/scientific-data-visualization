using UnityEngine;
using UnityEngine.EventSystems;

namespace TableTop {
    
    [RequireComponent(typeof(BoxCollider))]
    public class Draggable : MonoBehaviour
    {

        private Vector3 screenPoint;

        private Vector3 offset;

        private Vector3 originalPos;

        private Vector3 originalRot;

        private Camera maincam;

        private RayOnPanels rayOnPanels;

        private ExchangeTask exchangeTask;

        private Panel originPanel;

        private Panel targetPanel;

        private void Start()
        {
            maincam = Camera.main;

            rayOnPanels = RayOnPanels.Instance;

            exchangeTask = ExchangeTask.Instance;

            originPanel = transform.parent.gameObject.GetComponent<Panel>();
        }

        void OnMouseDown()

        {
            originalPos = transform.position;

            originalRot = transform.eulerAngles;


            screenPoint = maincam.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - maincam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        }

        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = maincam.ScreenToWorldPoint(curScreenPoint) + offset;
            
            transform.position = curPosition;

            transform.rotation = maincam.transform.rotation;

            transform.RotateAround(transform.position, transform.right, -90);

        }

        void OnMouseUp()
        {
            RaycastHit? hit = rayOnPanels.PanelHit();

            if (hit != null)
            {

               targetPanel = hit.Value.collider.gameObject.transform.parent.GetComponent<Panel>();

                if (targetPanel == originPanel) {

                    reinstateOriginalPosRot();

                }
                else { 

                    exchangeTask.Exchange(originPanel, targetPanel, this.gameObject.name);

                }
            }
            else {

                reinstateOriginalPosRot();

            }

        }

        private void reinstateOriginalPosRot() {

            transform.position = originalPos;

            transform.eulerAngles = originalRot;
        }

    }
}