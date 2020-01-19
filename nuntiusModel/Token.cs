using System;

namespace nuntiusModel
{
   public class Token
   {
       private byte[] token;

       public void GenerateToken()
       {
       }

        #region Properties
       public byte[] Bytes
       {
           get { return token; }
           set { token = value; }
       }
       #endregion
   } 
}