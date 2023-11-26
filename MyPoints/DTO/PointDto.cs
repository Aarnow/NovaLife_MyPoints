using System.Collections.Generic;
using static PointActionManager;

namespace MyPoints.DTO
{
    public class PointDto
    {
        public string Name { get; set; }
        public string DataFilePath { get; set; }
        public PointActionKeys ActionKey { get; set; }
        public bool IsOpen { get; set; }
        public List<int> AllowedBizs { get; set; } = new List<int>();
    }
}
