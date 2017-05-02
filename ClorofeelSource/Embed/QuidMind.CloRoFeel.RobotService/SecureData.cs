using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel
{
    static class SecureData
    {
        // This Data are spécific to your Azure AppFabric subscription

        #region Secure Data
        public static readonly string IssuerName = "owner";

        public static readonly string IssuerSecret =
            #region HIDDEN
 "TphX/HYzx5UKqui+FkdM3nBo1hpaPfkCsNK1kcf6U7Q=";
            #endregion

        public static readonly string AdminPhoneId = 
            #region HIDDEN
                "5BE9FF477FA04B06A846AFA3FFFFFFFF";
            #endregion
        #endregion


    }
}
