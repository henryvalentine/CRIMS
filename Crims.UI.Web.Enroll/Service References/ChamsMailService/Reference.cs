﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crims.UI.Web.Enroll.ChamsMailService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChamsMailService.WebServiceSoap")]
    public interface WebServiceSoap {
        
        // CODEGEN: Generating message contract since element name to from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/sendmail1", ReplyAction="*")]
        Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response sendmail1(Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/sendmail1", ReplyAction="*")]
        System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response> sendmail1Async(Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request request);
        
        // CODEGEN: Generating message contract since the wrapper name (sendmailWithAttachment) of message sendmailWithAttachment does not match the default value (sendmail)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/sendmailWithAttachment", ReplyAction="*")]
        Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1 sendmail(Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/sendmailWithAttachment", ReplyAction="*")]
        System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1> sendmailAsync(Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sendmail1Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="sendmail1", Namespace="http://tempuri.org/", Order=0)]
        public Crims.UI.Web.Enroll.ChamsMailService.sendmail1RequestBody Body;
        
        public sendmail1Request() {
        }
        
        public sendmail1Request(Crims.UI.Web.Enroll.ChamsMailService.sendmail1RequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class sendmail1RequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string to;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string subject;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string body;
        
        public sendmail1RequestBody() {
        }
        
        public sendmail1RequestBody(string to, string subject, string body) {
            this.to = to;
            this.subject = subject;
            this.body = body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class sendmail1Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="sendmail1Response", Namespace="http://tempuri.org/", Order=0)]
        public Crims.UI.Web.Enroll.ChamsMailService.sendmail1ResponseBody Body;
        
        public sendmail1Response() {
        }
        
        public sendmail1Response(Crims.UI.Web.Enroll.ChamsMailService.sendmail1ResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class sendmail1ResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool sendmail1Result;
        
        public sendmail1ResponseBody() {
        }
        
        public sendmail1ResponseBody(bool sendmail1Result) {
            this.sendmail1Result = sendmail1Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sendmailWithAttachment", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class sendmailWithAttachment {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string to;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string subject;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string body;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public string Path;
        
        public sendmailWithAttachment() {
        }
        
        public sendmailWithAttachment(string to, string subject, string body, string Path) {
            this.to = to;
            this.subject = subject;
            this.body = body;
            this.Path = Path;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="sendmailWithAttachmentResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class sendmailWithAttachment1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool sendmailWithAttachmentResult;
        
        public sendmailWithAttachment1() {
        }
        
        public sendmailWithAttachment1(bool sendmailWithAttachmentResult) {
            this.sendmailWithAttachmentResult = sendmailWithAttachmentResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebServiceSoapChannel : Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebServiceSoapClient : System.ServiceModel.ClientBase<Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap>, Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap {
        
        public WebServiceSoapClient() {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap.sendmail1(Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request request) {
            return base.Channel.sendmail1(request);
        }
        
        public bool sendmail1(string to, string subject, string body) {
            Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request inValue = new Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request();
            inValue.Body = new Crims.UI.Web.Enroll.ChamsMailService.sendmail1RequestBody();
            inValue.Body.to = to;
            inValue.Body.subject = subject;
            inValue.Body.body = body;
            Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response retVal = ((Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap)(this)).sendmail1(inValue);
            return retVal.Body.sendmail1Result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response> Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap.sendmail1Async(Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request request) {
            return base.Channel.sendmail1Async(request);
        }
        
        public System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmail1Response> sendmail1Async(string to, string subject, string body) {
            Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request inValue = new Crims.UI.Web.Enroll.ChamsMailService.sendmail1Request();
            inValue.Body = new Crims.UI.Web.Enroll.ChamsMailService.sendmail1RequestBody();
            inValue.Body.to = to;
            inValue.Body.subject = subject;
            inValue.Body.body = body;
            return ((Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap)(this)).sendmail1Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1 Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap.sendmail(Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment request) {
            return base.Channel.sendmail(request);
        }
        
        public bool sendmail(string to, string subject, string body, string Path) {
            Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment inValue = new Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment();
            inValue.to = to;
            inValue.subject = subject;
            inValue.body = body;
            inValue.Path = Path;
            Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1 retVal = ((Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap)(this)).sendmail(inValue);
            return retVal.sendmailWithAttachmentResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1> Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap.sendmailAsync(Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment request) {
            return base.Channel.sendmailAsync(request);
        }
        
        public System.Threading.Tasks.Task<Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment1> sendmailAsync(string to, string subject, string body, string Path) {
            Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment inValue = new Crims.UI.Web.Enroll.ChamsMailService.sendmailWithAttachment();
            inValue.to = to;
            inValue.subject = subject;
            inValue.body = body;
            inValue.Path = Path;
            return ((Crims.UI.Web.Enroll.ChamsMailService.WebServiceSoap)(this)).sendmailAsync(inValue);
        }
    }
}
