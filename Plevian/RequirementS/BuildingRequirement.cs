using Plevian.Buildings;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.RequirementS
{
    public class BuildingRequirement : Requirement
    {
        public BuildingType _buildingType;
        public int _desiredLevel;

        public BuildingRequirement(BuildingType buildingType, int desiredLevel)
        {
            this.BuildingType = buildingType;
            this.DesiredLevel = desiredLevel;
        }

        public override bool isFullfilled(Village village)
        {
            Building building = village.getBuilding(_buildingType);
            if (building != null && building.level >= _desiredLevel) ;
                return true;
            return false;
        }

        public override string ToString()
        {
            return "Requirement - " + Enum.GetName(typeof(BuildingType), _buildingType) + " lvl " + _desiredLevel;
        }

#region properties
        public int DesiredLevel
        {
            private set
            {
                _desiredLevel = value;
            }

            get
            {
                return _desiredLevel;
            }
        }

        public BuildingType BuildingType
        {
            private set
            {
                _buildingType = value;
            }

            get
            {
                return _buildingType;
            }
        }
#endregion
    }
}
