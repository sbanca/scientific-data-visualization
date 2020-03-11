using System.Threading.Tasks;

namespace TableTop
{
    public class RoutesDisplay : Singleton<RoutesDisplay>
    {
        

        public async Task DisplayRoutes(PannelTasks tasklist) {


            await tasklist.Update();

            Routes.Instance.DeactivateAll();


            foreach (RouteData rs in tasklist.SelectedRoutes) 
                
                Routes.Instance.Add(rs);


            foreach (RouteData rs in tasklist.OptionalRoutes) 
                
                Routes.Instance.Add(rs);

        }

      

    }


    

}

