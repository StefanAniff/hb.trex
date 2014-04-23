using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Trex.Server.Core.Model;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class UserCreationResponse:ServerResponse
    {

        public UserCreationResponse(string responseMessage, bool success)
            : base(responseMessage, success)
        {


        }

        [DataMember]
        public User User { get; set; }
    }
}