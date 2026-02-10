<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrystalReport.aspx.cs" Inherits="WebReportHightJump.CrystalReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        /*html#html, body#body, form#form1, div#content, center#center {
            border: 0px solid black;
            padding: 0px;
            margin: 0px;
            height: 100%;
        }*/

        .btn-floating-container {
            right: 30px;
            top: 30px;
            position: fixed;
            z-index: 9999999;
        }

        .btn-floating {
            /* width: 50px; */
            /* height: 60px; */
            border-radius: 50%;
            box-shadow: 0 2px 6px 0 rgba(0, 0, 0, 0.2), 0 1px 1px 0 rgba(0, 0, 0, 0.2);
            border: 1px solid rgba(245, 245, 245, 0.075);
            /* text-align: center; */
            /* padding: 0px; */
            /* font-size: 24px; */
            display: block;
            background-color: rgba(255, 255, 255, 0.25882352941176473);
        }


        /* The container */
        .container-checkbox {
            display: block;
            position: relative;
            padding-left: 35px;
            padding-top: 7px;
            margin-bottom: 12px;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default checkbox */
            .container-checkbox input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
            }

        /* Create a custom checkbox */
        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 25px;
            width: 25px;
            background-color: #eee;
            border: solid 1px #bbbbbb;
        }

        /* On mouse-over, add a grey background color */
        .container-checkbox:hover input ~ .checkmark {
            background-color: #ccc;
        }

        /* When the checkbox is checked, add a blue background */
        .container-checkbox input:checked ~ .checkmark {
            background-color: #2196F3;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the checkmark when checked */
        .container-checkbox input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the checkmark/indicator */
        .container-checkbox .checkmark:after {
            left: 9px;
            top: 5px;
            width: 5px;
            height: 10px;
            border: solid white;
            border-width: 0 3px 3px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }

        .modal {
            text-align: center;
        }

        @media screen and (min-width: 768px) {
            .modal:before {
                display: inline-block;
                vertical-align: top;
                content: " ";
                height: 100%;
            }
        }

        .modal-dialog {
            display: inline-block;
            text-align: left;
            vertical-align: top;
        }

        .modal-header {
            background: #0d2143;
        }

            .modal-header h3, .modal-header button {
                color: #fff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="alert alert-danger alert-dismissible fade show" role="alert" runat="server" id="alert_box">
        <asp:Label ID="lbError" runat="server" Text="Label"></asp:Label>

        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>


    <div id="printInstallnModal" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 id="printOptionModalLabel">WCPP utility</h3>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="msgInProgress" style="font-size: 11pt;" class="p-4">
                        <h3>Detecting WCPP utility at client side...</h3>

                        <div class="alert">
                            <span class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></span>
                            <h3>
                                <%--<asp:Image ID="imgSpinner" runat="server" BorderWidth="0px" BorderStyle="None" />--%>
                                <i class="fa fa-spinner fa-pulse"></i>
                                Please wait a few seconds...</h3>
                        </div>
                    </div>

                    <div id="msgInstallWCPP" class="alert alert-error" style="display: none;">
                        <div class="container">
                            <div class="row">
                                <div class="col">
                                    <h2>คำเตือน!</h2>
                                    <span style="font-size: 14pt;">กรุณาดาวน์โหลดและติดตั้งโปรแกรมสำหรับการพิมพ์</span>
                                    <br />
                                    <span style="font-size: 14pt;">* เมื่อติดตั้งเรียบร้อยแล้วให้ทำการ refresh หน้าเว็บ</span>
                                </div>
                            </div>
                            <div class="row pt-4">
                                <div class="col-md-6 offset-md-6">
                                    <asp:LinkButton ID="btInstall" runat="server" CssClass="btn btn-success col-12" OnClick="btInstall_Click">ดาวน์โหลดและติดตั้ง</asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>



    <%--****************************** Printer Option *****************************--%>

    <!-- Modal -->
    <div id="printOptionModal" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog  modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 id="printOptionModalLabel">Print Option</h3>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--   <div>
                        <label class="container-checkbox">
                            <strong>พิมพ์ไปยังเครื่องพิมพ์เริ่มต้น (Default printer)</strong> หรือ...
                        <input type="checkbox" id="useDefaultPrinter" />
                            <span class="checkmark"></span>
                        </label>
                    </div>--%>

                    <div class="row">
                        <div class="col-5">
                            <div class="row">
                                <div class="col">
                                    <label class="container-checkbox">
                                        <strong>Print to Default printer</strong> or...
                                    <input type="checkbox" id="useDefaultPrinter" />
                                        <span class="checkmark"></span>
                                    </label>
                                </div>
                            </div>

                            <div class="row" id="spinnerGetPrinter" style="display: none">
                                <div class="col">
                                    <div>
                                        <div class="alert alert-info" role="alert">
                                            <i class="fa fa-spinner fa-pulse"></i>&nbsp;Please wait a few seconds...
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" id="loadPrinters">
                                <div class="col">
                                    Click to load and select one of the installed printers! 
                                    <br />
                                    <input type="button" class="btn btn-warning" onclick="javascript: jsWebClientPrint.getPrintersInfo();" value="Load installed printers..." />
                                    <br />
                                    <br />
                                </div>
                            </div>

                            <div class="row" id="installedPrinters" style="visibility: hidden;">
                                <div class="col">
                                    <label for="installedPrinterName">Printers :</label>
                                    <select name="installedPrinterName" id="installedPrinterName" onchange="showSelectedPrinterInfo();" class="form-control"></select>
                                </div>
                            </div>


                        </div>

                        <div class="col-1 border-right"></div>

                        <div class="col-6">
                            <div class="row">
                                <div class="col">
                                    <label for="lstPrinterTrays">Supported Trays:</label>
                                    <select name="lstPrinterTrays" id="lstPrinterTrays" class="form-control"></select>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col">
                                    <label for="lstPrinterPapers">Supported Papers:</label>
                                    <select name="lstPrinterPapers" id="lstPrinterPapers" class="form-control"></select>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col">
                                    <label for="lstPrintRotation">Print Rotation (Clockwise):</label>
                                    <select name="lstPrintRotation" id="lstPrintRotation" class="form-control">
                                        <option>None</option>
                                        <option>Rot90</option>
                                        <option>Rot180</option>
                                        <option>Rot270</option>
                                    </select>
                                </div>
                            </div>

                          

                            <div class="row">
                                <div class="col">
                                    <hr />
                                </div>
                            </div>


                            <div class="accordion" id="accordionMoreSetting">
                                <div class="card">
                                    <div class="card-header p-0 m-0" id="headingOne">
                                        <h2 class="mb-0 p-0">
                                            <button class="btn" type="button" data-toggle="collapse" data-target="#collapseMoreSetting" aria-expanded="true" aria-controls="collapseMoreSetting" style="color: #838383; text-decoration: none;">
                                                More Settings
                                            </button>
                                        </h2>
                                    </div>

                                    <div id="collapseMoreSetting" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionMoreSetting">
                                        <div class="card-body">
                                              <div class="row">
                                                <div class="col">
                                                    <label for="txtCopies">Number of copies</label>
                                                    <input type="number" min="1" max="9999" class="form-control" id="txtCopies" value="1" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col">
                                                    <label for="txtPagesRange">Pages Range: [e.g. 1,2,3,10-15]</label>
                                                    <input type="text" class="form-control" id="txtPagesRange" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col">
                                                    <hr />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col">
                                                    <label class="container-checkbox">
                                                        Auto Rotate (Page Orientation)
                                                        <input type="checkbox" id="chkPrintAutoRotate" checked="checked" />
                                                        <span class="checkmark"></span>
                                                    </label>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col">
                                                    <label class="container-checkbox">
                                                        Print In Reverse Order
                                                        <input type="checkbox" id="chkPrintInReverseOrder" />
                                                        <span class="checkmark"></span>
                                                    </label>

                                                    <%--<div class="checkbox">
                                                        <label>
                                                            <input id="chkPrintInReverseOrder" type="checkbox" value="" />Print In Reverse Order?</label>
                                                    </div>--%>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col">
                                                    <label class="container-checkbox">
                                                        Print Annotations
                                                        <input type="checkbox" id="chkPrintAnnotations" />
                                                        <span class="checkmark"></span>
                                                    </label>
                                                    <%--<div class="checkbox">
                                                        <label>
                                                            <input id="chkPrintAnnotations" type="checkbox" value="" />Print Annotations? <span class="label label-info">Windows Only</span></label>
                                                    </div>--%>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col">
                                                    <label class="container-checkbox">
                                                        Print As Grayscale
                                                        <input type="checkbox" id="chkPrintAsGrayscale" />
                                                        <span class="checkmark"></span>
                                                    </label>
                                                    <%--<div class="checkbox">
                                                        <label>
                                                            <input id="chkPrintAsGrayscale" type="checkbox" value="" />Print As Grayscale? <span class="label label-info">Windows Only</span></label>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-outline-secondary" data-dismiss="modal" aria-hidden="true"><i class="fa fa-times-circle"></i>&nbsp;Close</button>
                    <button class="btn btn-primary" type="button" onclick="printReport()"><i class="fa fa-print"></i>&nbsp;Print</button>
                </div>
            </div>
        </div>
    </div>


    <%--******************************Report Viewer*******************************--%>
    <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="auto">
        <div id="dvReport">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                Height="900px" Width="100%"
                HasToggleGroupTreeButton="false"
                ToolPanelWidth="200px"
                EnableParameterPrompt="False" BestFitPage="False"
                ToolPanelView="None" />
        </div>

        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="CrystalReports\Report13_.rpt" />
        </CR:CrystalReportSource>
    </asp:Panel>


    <script type="text/javascript">
        var wcppGetPrintersTimeout_ms = 10000; //10 sec
        var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec

        var clientPrinters = null;

        function wcpGetPrintersOnSuccess() {
            $('#spinnerGetPrinter').css('display', 'none');
            // Display client installed printers
            if (arguments[0].length > 0) {
                if (JSON) {
                    try {
                        clientPrinters = JSON.parse(arguments[0]);
                        if (clientPrinters.error) {
                            alert(clientPrinters.error)
                        } else {
                            var options = '';
                            for (var i = 0; i < clientPrinters.length; i++) {
                                options += '<option>' + clientPrinters[i].name + '</option>';
                            }
                            $('#lstPrinters').html(options);
                            $('#lstPrinters').focus();

                            showSelectedPrinterInfo();
                        }
                    } catch (e) {
                        alert(e.message)
                    }
                }

                //var p = arguments[0].split("|");
                //var options = '';
                //for (var i = 0; i < p.length; i++) {
                //    options += '<option>' + p[i] + '</option>';
                //}
                $('#installedPrinters').css('visibility', 'visible');
                $('#installedPrinterName').html(options);
                $('#installedPrinterName').focus();
                $('#loadPrinters ').hide();
            } else {
                alert("No printers are installed in your system. ");
            }
        }
        function wcpGetPrintersOnFailure() {
            // Do something if printers cannot be got from the client
            alert("No printers are installed in your system.");
        }

        function showSelectedPrinterInfo() {
            // get selected printer index
            var idx = $("#installedPrinterName")[0].selectedIndex;
            // get supported trays
            var options = '';
            if (clientPrinters[idx] && clientPrinters[idx].trays) {
                for (var i = 0; i < clientPrinters[idx].trays.length; i++) {
                    options += '<option>' + clientPrinters[idx].trays[i] + '</option>';
                }
            }
            $('#lstPrinterTrays').html(options);
            // get supported papers
            options = '';
            if (clientPrinters[idx] && clientPrinters[idx].papers) {
                for (var i = 0; i < clientPrinters[idx].papers.length; i++) {
                    options += '<option>' + clientPrinters[idx].papers[i] + '</option>';
                }
            }
            $('#lstPrinterPapers').html(options);
        }


        function printReport() {
            var query = '<%=HttpUtility.UrlEncode(Request.QueryString.ToString())%>';

            jsWebClientPrint.print('useDefaultPrinter=' + $('#useDefaultPrinter').attr('checked')
                    + '&printerName=' + encodeURIComponent($('#installedPrinterName').val())
                    + '&trayName=' + encodeURIComponent($('#lstPrinterTrays').val())
                    + '&paperName=' + encodeURIComponent($('#lstPrinterPapers').val())
                    + '&printRotation=' + $('#lstPrintRotation').val()
                    + '&pagesRange=' + encodeURIComponent($('#txtPagesRange').val())
                    + '&printAnnotations=' + $('#chkPrintAnnotations').prop('checked')
                    + '&printAsGrayscale=' + $('#chkPrintAsGrayscale').prop('checked')
                    + '&printInReverseOrder=' + $('#chkPrintInReverseOrder').prop('checked')
                    + '&printAutoRotate=' + $('#chkPrintAutoRotate').prop('checked')
                    + '&printCopies=' + encodeURIComponent($('#txtCopies').val())
                    + '&' + query //encodeURIComponent('<%//Request.QueryString.ToString()%>')
                );

             $('#printOptionModal').modal('hide');
        }
    </script>

    <script type="text/javascript">

        var wcppPingTimeout_ms = 10000; //10 sec
        var wcppPingTimeoutStep_ms = 500; //0.5 sec

        function wcppDetectOnSuccess() {
            // WCPP utility is installed at the client side
            // redirect to WebClientPrint sample page

            // get WCPP version
            var wcppVer = arguments[0];
            if (wcppVer.substring(0, 1) == "5") {
                //window.location.href = 'PrintRPT.aspx';
                console.log("Print Client Detected");
                $('#msgInProgress').hide();
                //$('#msgInstallWCPP').hide();

                $('#printInstallnModal').modal('hide');

                //$('#printOptionModal').modal('show');
            }
            else //force to install WCPP v4.0
                wcppDetectOnFailure();
        }

        function wcppDetectOnFailure() {
            // It seems WCPP is not installed at the client side
            // ask the user to install it
            $('#msgInProgress').hide();
            $('#msgInstallWCPP').show();

            $('#printInstallnModal').modal('show');
        }
    </script>


    <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(
                              (System.Configuration.ConfigurationManager.AppSettings["ReportProtocol"] != null ?System.Configuration.ConfigurationManager.AppSettings["ReportProtocol"] : HttpContext.Current.Request.Url.Scheme) + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/WebPrint" + "/WebClientPrintAPI.ashx"
                            , (System.Configuration.ConfigurationManager.AppSettings["ReportProtocol"] != null ?System.Configuration.ConfigurationManager.AppSettings["ReportProtocol"] : HttpContext.Current.Request.Url.Scheme) + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/WebPrint" + "/PrintRPTHandler.ashx"
                            , HttpContext.Current.Session.SessionID)    
    %>
</asp:Content>
