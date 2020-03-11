using System;
using System.Threading.Tasks;


namespace TableTop
{
  
    [Serializable]
    public class RouteData 
    {
        //type and name
        public string name;
        public RouteType type;

        //start coordinates
        private OptionItem _startOption;
        private Mapzen.LngLat start;
        public OptionItem startOption
        {

            get { return _startOption; }
            set
            {

                _startOption = value;

                start = new Mapzen.LngLat(_startOption.Lng, _startOption.Lat);

            }

        }

        //end coordinates
        private OptionItem _endOption;
        private Mapzen.LngLat end;
        public OptionItem endOption
        {

            get { return _endOption; }
            set
            {

                _endOption = value;

                end = new Mapzen.LngLat(_endOption.Lng, _endOption.Lat);

            }
        }

        //route specifics
        public float duration;
        public float distance;
        public double[][] coordinatesRoute;

        public RouteData(OptionItem start, OptionItem end, RouteType type)
        {
            this.startOption = start;
            this.endOption = end;
            this.type = type;
            this.name = start.Name + "_" + end.Name;

        }

        public async Task apiCall()
        {

            Response response = await OpenRouteService.Instance.Direction(start, end);

            coordinatesRoute = response.features[0].geometry.coordinatesRoute;

            duration = response.features[0].properties.summary.duration;

            distance = response.features[0].properties.summary.distance;

        }


    }



}