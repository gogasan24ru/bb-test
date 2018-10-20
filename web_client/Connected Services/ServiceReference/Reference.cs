﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web_client.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IUsersManagementService")]
    public interface IUsersManagementService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Test", ReplyAction="http://tempuri.org/IUsersManagementService/TestResponse")]
        bool Test(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Test", ReplyAction="http://tempuri.org/IUsersManagementService/TestResponse")]
        System.Threading.Tasks.Task<bool> TestAsync(string input);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/ListUsers", ReplyAction="http://tempuri.org/IUsersManagementService/ListUsersResponse")]
        ClassLibrary1.Returnable ListUsers(uint timestamp, string sessionKey, byte[] hash, int page, string filterSet);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/ListUsers", ReplyAction="http://tempuri.org/IUsersManagementService/ListUsersResponse")]
        System.Threading.Tasks.Task<ClassLibrary1.Returnable> ListUsersAsync(uint timestamp, string sessionKey, byte[] hash, int page, string filterSet);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Login", ReplyAction="http://tempuri.org/IUsersManagementService/LoginResponse")]
        ClassLibrary1.Returnable Login(uint timestamp, [System.ServiceModel.MessageParameterAttribute(Name="login")] string login1, string password, byte[] hash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Login", ReplyAction="http://tempuri.org/IUsersManagementService/LoginResponse")]
        System.Threading.Tasks.Task<ClassLibrary1.Returnable> LoginAsync(uint timestamp, string login, string password, byte[] hash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Logout", ReplyAction="http://tempuri.org/IUsersManagementService/LogoutResponse")]
        ClassLibrary1.Returnable Logout(uint timestamp, string sessionKey, byte[] hash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Logout", ReplyAction="http://tempuri.org/IUsersManagementService/LogoutResponse")]
        System.Threading.Tasks.Task<ClassLibrary1.Returnable> LogoutAsync(uint timestamp, string sessionKey, byte[] hash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Register", ReplyAction="http://tempuri.org/IUsersManagementService/RegisterResponse")]
        ClassLibrary1.Returnable Register(uint timestamp, ClassLibrary1.User data, byte[] hash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUsersManagementService/Register", ReplyAction="http://tempuri.org/IUsersManagementService/RegisterResponse")]
        System.Threading.Tasks.Task<ClassLibrary1.Returnable> RegisterAsync(uint timestamp, ClassLibrary1.User data, byte[] hash);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUsersManagementServiceChannel : web_client.ServiceReference.IUsersManagementService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UsersManagementServiceClient : System.ServiceModel.ClientBase<web_client.ServiceReference.IUsersManagementService>, web_client.ServiceReference.IUsersManagementService {
        
        public UsersManagementServiceClient() {
        }
        
        public UsersManagementServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UsersManagementServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UsersManagementServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UsersManagementServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Test(string input) {
            return base.Channel.Test(input);
        }
        
        public System.Threading.Tasks.Task<bool> TestAsync(string input) {
            return base.Channel.TestAsync(input);
        }
        
        public ClassLibrary1.Returnable ListUsers(uint timestamp, string sessionKey, byte[] hash, int page, string filterSet) {
            return base.Channel.ListUsers(timestamp, sessionKey, hash, page, filterSet);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary1.Returnable> ListUsersAsync(uint timestamp, string sessionKey, byte[] hash, int page, string filterSet) {
            return base.Channel.ListUsersAsync(timestamp, sessionKey, hash, page, filterSet);
        }
        
        public ClassLibrary1.Returnable Login(uint timestamp, string login1, string password, byte[] hash) {
            return base.Channel.Login(timestamp, login1, password, hash);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary1.Returnable> LoginAsync(uint timestamp, string login, string password, byte[] hash) {
            return base.Channel.LoginAsync(timestamp, login, password, hash);
        }
        
        public ClassLibrary1.Returnable Logout(uint timestamp, string sessionKey, byte[] hash) {
            return base.Channel.Logout(timestamp, sessionKey, hash);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary1.Returnable> LogoutAsync(uint timestamp, string sessionKey, byte[] hash) {
            return base.Channel.LogoutAsync(timestamp, sessionKey, hash);
        }
        
        public ClassLibrary1.Returnable Register(uint timestamp, ClassLibrary1.User data, byte[] hash) {
            return base.Channel.Register(timestamp, data, hash);
        }
        
        public System.Threading.Tasks.Task<ClassLibrary1.Returnable> RegisterAsync(uint timestamp, ClassLibrary1.User data, byte[] hash) {
            return base.Channel.RegisterAsync(timestamp, data, hash);
        }
    }
}
