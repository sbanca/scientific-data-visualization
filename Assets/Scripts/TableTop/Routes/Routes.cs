using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableTop
{
    public class Routes : Singleton<Routes>
    {

        public List<Route> routes = new List<Route> { };

        //this method creates or activate a route
        public void Add(OptionItem startOption, OptionItem endOption, RouteType type) {

            lock (routes)
            {
                Route retrievedRoute = null;

                string entryName = startOption.Name + "_" + endOption.Name;

                if (routes.Count > 0)
                {
                    retrievedRoute = this.routes.FirstOrDefault(entry => entry.name.Equals(entryName)); 
                   
                }

                if (retrievedRoute == null)
                {
                    var newGameobject = new GameObject();

                    var newRoute = Route.CreateComponent(newGameobject,startOption, endOption, type);

                    newRoute.Generate();

                    routes.Add(newRoute);

                }
                else
                {
                    
                    retrievedRoute.gameObject.SetActive(true);//make sure the object is active

                    retrievedRoute.type = type; //update type will trigger material change

                }
               
            }

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