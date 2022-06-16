using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerUI1
{
    public interface IPrizeReqeuster
    {
        void PrizeComplete(PrizeModel model);
    }
}
