using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralLibrary.Helpers.Data_Operations
{
    public static class NormalizeData
    {
        #region Properties
        public static double Min { get; set; }
        public static double Max { get; set; }
        #endregion
        #region Method

        public static bool NormalizeInput(List<double> nums)// 0 to 1 Norm
        {
            bool IsNormalized = false;
            bool listNowNormalized = false;
            Min = 0.0; Max= 0.0;
            foreach (double n in nums)
            {
                if (Min  > n)
                {
                    Min = n;
                }
                if (Max < n)
                {
                    Max = n;
                }
            }
            if (Max == 1 && Min == 0)
            { IsNormalized = true; listNowNormalized = true; }
            if(!IsNormalized)
            {
                double distance = Max - Min;
                for (int i = 0; i < nums.Count; i++)
                {
                    nums[i] = (nums[i] - Min) / distance;
                }
                listNowNormalized = true;
            }
            return listNowNormalized;
        }
            
        #endregion
    }
}
