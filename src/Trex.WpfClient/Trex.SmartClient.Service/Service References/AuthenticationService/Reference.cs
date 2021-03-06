﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trex.SmartClient.Service.AuthenticationService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="AuthenticationService.IAuthenticationService")]
    public interface IAuthenticationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:IAuthenticationService/ValidateUser", ReplyAction="urn:IAuthenticationService/ValidateUserResponse")]
        bool ValidateUser(string userName, string password, string customerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:IAuthenticationService/ValidateUser", ReplyAction="urn:IAuthenticationService/ValidateUserResponse")]
        System.Threading.Tasks.Task<bool> ValidateUserAsync(string userName, string password, string customerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:IAuthenticationService/ResetPassword", ReplyAction="urn:IAuthenticationService/ResetPasswordResponse")]
        bool ResetPassword(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:IAuthenticationService/ResetPassword", ReplyAction="urn:IAuthenticationService/ResetPasswordResponse")]
        System.Threading.Tasks.Task<bool> ResetPasswordAsync(string userName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAuthenticationServiceChannel : Trex.SmartClient.Service.AuthenticationService.IAuthenticationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthenticationServiceClient : System.ServiceModel.ClientBase<Trex.SmartClient.Service.AuthenticationService.IAuthenticationService>, Trex.SmartClient.Service.AuthenticationService.IAuthenticationService {
        
        public AuthenticationServiceClient() {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthenticationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool ValidateUser(string userName, string password, string customerId) {
            return base.Channel.ValidateUser(userName, password, customerId);
        }
        
        public System.Threading.Tasks.Task<bool> ValidateUserAsync(string userName, string password, string customerId) {
            return base.Channel.ValidateUserAsync(userName, password, customerId);
        }
        
        public bool ResetPassword(string userName) {
            return base.Channel.ResetPassword(userName);
        }
        
        public System.Threading.Tasks.Task<bool> ResetPasswordAsync(string userName) {
            return base.Channel.ResetPasswordAsync(userName);
        }
    }
}
