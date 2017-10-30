#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: PosterParameters.cs
//
#endregion

namespace MTV.EventDispatcher.Service.Extensions.PosterExtension
{
    public interface PosterParameters
    {
        #region -.-.-.-.-.-.-.-.-.-.- Class : Proprity(ies) -.-.-.-.-.-.-.-.-.-.-
        
        int ImgOutputWidth { get; set; }
        int ImgOutputHeight { get; set; }
        string PosterAutomaticRecordLastSent { get; set; }
        
        #endregion
    }
}
