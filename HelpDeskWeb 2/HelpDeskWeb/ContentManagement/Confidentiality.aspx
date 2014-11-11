<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Confidentiality.aspx.cs" Inherits="HelpDeskWeb.ContentManagement.Confidentiality" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <form runat="server">
        <div class="row">
                <div class="col-lg-6 col-lg-offset-3">
                    <div class="jumbotron">
                        <h1>Confidentiality Agreement</h1>
                        <p>College of the Ozarks Student Worker</p>
                    </div>
                    <div class="panel panel-default">
                      <div class="panel-heading">
                        <h3 class="panel-title">Confidentiality</h3>
                      </div>
                      <div class="panel-body">
                        <p>I understand that as a student worker I have access to confidential information about College applicants, students, employees, 
                            policies and procedures. This information may include academic information, (e.g. grades), financial information, (e.g. tax returns), 
                            or personal information (e.g. parent’s names, reference letters). This information may be obtained from observations, conversations, 
                            correspondence, personal records, clerical materials, the computer database or generated computer reports. It is my responsibility to: 
                        </p>
                            <ol type="1" class="list-group">
                                  <li class="list-group-item">Protect the privacy of individuals about whom I have confidential information;</li>
                                  <li class="list-group-item">Refrain from discussing matters pertaining to the office/department I am working for with, or in the presence of, non-office persons;</li>
                                  <li class="list-group-item">Limit my access to confidential information to that for which I have a work-related need;</li>
                                  <li class="list-group-item">Protect my password;</li>
                                  <li class="list-group-item">Protect written records from viewing by unauthorized individuals;</li>
                                  <li class="list-group-item">Protect computer records by exiting screens containing personal information, and logging off when I leave my work station.</li>
                            </ol>
                          <p>
                              Confidential information will be disclosed only to institutional offices or officials who have a legitimate need to know or with the written approval of the student.
                              I understand that the intentional unauthorized disclosure of confidential information could subject me to criminal and civil penalties imposed by law.
                              I further acknowledge that such willful or unauthorized disclosure also violates College policy and could constitute just cause for disciplinary action. 
                          </p>
                         <p>
                             By clicking the Agree button, I indicate that I have read and understand 
                         </p>
                            <ol type="1" class="list-group">
                                  <li class="list-group-item"><a class="btn btn-link" href="#">This Confidentiality Agreement,</a></li>
                                  <li class="list-group-item"><a class="btn btn-link" href="http://www.cofo.edu/Page/About-C-of-O/Policies-Legal-Notices/Computer-Use-Policy.330.html" role="button">The Computer Use Policy,</a></li>
                                  <li class="list-group-item"><a class="btn btn-link" href="http://www.cofo.edu/Page/About-C-of-O/Policies-Legal-Notices/STUDENT-INTERNET-ACCESS-POLICY.331.html" role="button">The Student Internet Access Policy,</a></li>
                                  <li class="list-group-item"><a class="btn btn-link" href="http://www.cofo.edu/Page/About-C-of-O/Consumer-Information/Student-Education-Records-and-Family-Educational-Rights-and-Privacy-Act.336.html" role="button">The FERPA Brochure,</li>
                            </ol>
                          <p>
                              and agree to abide by them.
                          </p>
                     </div> 
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2 col-sm-offset-5">
                     <div class="form-horizontal">
                            <asp:Button ID="Agree" runat="server" Text="I Agree" CssClass="btn btn-block btn-success" OnClick="Agree_Click" />

                            <br />
                            <asp:Button ID="Decline" runat="server" Text="Decline" CssClass="btn btn-block btn-danger" OnClick="Decline_Click" />
                        </div>
                </div>
            </div>
    </form>
    
     
 <%--   <asp:Button ID="Agree" runat="server" Text="I Agree" OnClick="Agree_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Decline" runat="server" Text="Decline" OnClick="Decline_Click" />
        <br />
        <br />
        <asp:Label ID="ErrorInfo" runat="server" ForeColor="Red"></asp:Label>
    </center>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">

 p.MsoNormal
	{margin-bottom:.0001pt;
	font-size:12.0pt;
	font-family:"Courier New";
	        margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }
ol
	{margin-bottom:0in;}
 li.MsoNormal
	{margin-bottom:.0001pt;
	font-size:12.0pt;
	font-family:"Courier New";
	        margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }
    </style>--%>
</asp:Content>
