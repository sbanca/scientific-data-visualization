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
        private OptionData _startOption;
        private Mapzen.LngLat start;
        public OptionData startOption
        {

            get { return _startOption; }
            set
            {

                _startOption = value;

                start = new Mapzen.LngLat(_startOption.Lng, _startOption.Lat);

            }

        }

        //end coordinates
        private OptionData _endOption;
        private Mapzen.LngLat end;
        public OptionData endOption
        {

            get { return _endOption; }
            set
            {

                _endOption = value;

                end = new Mapzen.LngLat(_endOption.Lng, _endOption.Lat);

            }
        }

        //route specifics
        public float departure;
        public float duration;
        public float arrival;
        public float distance;
        public float pollution;

        public double[][] coordinatesRoute;

        public RouteData(OptionData start, OptionData end, RouteType type)
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