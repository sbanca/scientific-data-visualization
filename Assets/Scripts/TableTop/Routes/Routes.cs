using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace TableTop
{
    public class Routes : Singleton<Routes>
    {


        public List<RouteManager> routes = new List<RouteManager> { };

       
        public void Add(RouteData routedata) {

            RouteManager retrievedRoute = null;

            RouteManager newRoute = null;

            lock (routes)
            {
                
                string entryName = routedata.name;

                if (routes.Count > 0)
                {
                    retrievedRoute = this.routes.FirstOrDefault(entry => entry.routeData.name.Equals(entryName)); 
                   
                }

                if (retrievedRoute == null)
                {
                    var newGameobject = new GameObject();

                    newRoute = RouteManager.CreateComponent(newGameobject, routedata);

                    routes.Add(newRoute);

                }
                else
                {
                    
                    retrievedRoute.gameObject.SetActive(true); //make sure the object is active

                    retrievedRoute.type = routedata.type; //update type will trigger material change

                }               

            }

            if (newRoute != null) 
                
                newRoute.CreateMesh(); //asynch task must be outside lock 

        }

        public void DeactivateAll() {

            

            lock (routes)
            {
                for (int i = 0; i < routes.Count; i++)
                {
                    routes.ElementAt(i).gameObject.SetActive(false);
                }
            }

        }

  
        public object DequeuInactive() {

            lock (routes)
            {
                for (int i = 0; i < routes.Count; i++)
                {
                    if (!routes[i].gameObject.activeSelf)
                    {
                        routes.RemoveAt(i);

                        return DequeuInactive();

                    }               

                }
            }

            return null;

        }

    }



}