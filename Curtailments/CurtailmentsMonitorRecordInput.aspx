<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurtailmentsMonitorRecordInput.aspx.cs" Inherits="Curtailments_CurtailmentsMonitorRecordInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="./StyleSheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
          <telerik:RadScriptManager runat="server" ID="scrptMgr" ></telerik:RadScriptManager>
        <div>
             <h1 style="text-align:center;">Curtailments Manual Record Input </h1><br />
        <div class="main">
                    <div>
                        <asp:Label runat ="server" >WTG : </asp:Label>
                         <telerik:RadComboBox RenderMode="Lightweight" ID="WTG" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="200px">
                              
                            </telerik:RadComboBox>
                    </div>
                    <div>
                         <asp:Label runat ="server" >Start Date/Time : </asp:Label>
                        <%--<telerik:RadDateTimePicker ID="From" runat="server" AutoPostBack="true"  DateInput-DateFormat="yyyy-MM-dd HH:mm"></telerik:RadDateTimePicker>--%>
                        <input type="datetime-local" class="DateTimePicker"  runat="server" id="From" width="250" height="35"/>
                    </div>
                    <div>
                         <asp:Label runat ="server" >End Date/Time : </asp:Label>
                        <%--<telerik:RadDateTimePicker ID="To" runat="server" AutoPostBack="true" Width="250" Height="35" DateInput-DateFormat="yyyy-MM-dd HH:mm"></telerik:RadDateTimePicker>--%>
                        <input type="datetime-local" class="DateTimePicker" runat="server" id="To" width="250" height="35"/>
                    </div>
                    <div>
                        <asp:Label runat ="server" >Set Point : </asp:Label>
                        <telerik:RadNumericTextBox ID="SetPoit" runat="server"   DecimalDigits="2" Width="200" ></telerik:RadNumericTextBox>
            

                    </div>

                    <div>
                       <asp:Button ID="btnAdd" CssClass="CMR" runat="server" Text="Add" OnClick="btnAdd_Click" />

                    </div>
                </div>
                <br /><br />

        <div>
             <telerik:RadGrid ID="gridView" runat="server"  AutoGenerateColumns="false"  >
                     <MasterTableView  >
                         <Columns>
                             <telerik:GridTemplateColumn HeaderText="S.NO">
                                   
                                    <ItemTemplate>
                                        <%# Container.ItemIndex + 1 %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="WTGName" HeaderText="WTGName" ></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DateFrom" HeaderText="DateFrom"  ></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DateTo" HeaderText="DateTo"   ></telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="SetPoint" HeaderText="SetPoint" ></telerik:GridBoundColumn>
                              <telerik:GridTemplateColumn HeaderText="Remove" >
                            <ItemTemplate>
                                <asp:Button CssClass="TableBtn" ID="btnRemove" runat="server" Text="Remove" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemove_Command" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="Remove" >
                            <ItemTemplate>
                                <asp:Button CssClass="TableBtn" ID="AddSubSet" runat="server" Text="Add Sub Set Point" CommandName="SubSet" CommandArgument="<%# Container.ItemIndex %>" OnCommand="AddSubSetPoint" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                    </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
        </div>
        
        <br /><br />

        <div class="main" id="SubSetInputs" runat="server" visible="false">
           <div>
               <asp:Label runat ="server" >WTG : </asp:Label>
                <telerik:RadComboBox RenderMode="Lightweight" ID="RadComboBox1" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="200px">
                    
                </telerik:RadComboBox>
            </div>
            <div>
                <asp:Label runat ="server" >Start Date/Time : </asp:Label>
               <%-- <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" AutoPostBack="true" Width="250" Height="35" DateInput-DateFormat="yyyy-MM-dd HH:mm"></telerik:RadDateTimePicker>--%>
                <input type="datetime-local" class="DateTimePicker" runat="server" id="RadDateTimePicker1" width="250" height="50"/>
            </div>
             <div>
                <asp:Label runat ="server" >Sub Set Point : </asp:Label>
                <telerik:RadNumericTextBox ID="RadNumericTextBox1" runat="server"   DecimalDigits="2" Width="200" ></telerik:RadNumericTextBox>
            
            </div>
            <div><asp:Label ID="labelEndDate"  runat ="server" >End Date/Time : </asp:Label>
               <%-- <telerik:RadDateTimePicker  ID="RadDateT" runat="server" AutoPostBack="true" Width="250" Height="35" DateInput-DateFormat="yyyy-MM-dd HH:mm"></telerik:RadDateTimePicker>--%>
                <input type="datetime-local" class="DateTimePicker" runat="server" id="RadDateT" width="250" height="35"/>
            </div>
            <div>
                <asp:Button ID="Button2" CssClass="CMR" runat="server" Text="Add" OnClick="Button2_Click" />

            </div>
           
        </div>
       
        <br /><br />
         <div>
             
              <telerik:RadGrid ID="RadGrid1" runat="server"  AutoGenerateColumns="false"  >
              <MasterTableView  >
                  <Columns>
                      <telerik:GridTemplateColumn HeaderText="S.NO">
      
                           <ItemTemplate>
                              <%# Container.ItemIndex +1 %>
                           </ItemTemplate>
                       </telerik:GridTemplateColumn>
                
                 <telerik:GridBoundColumn DataField="WTGName" HeaderText="WTGName" ></telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="DateFrom" HeaderText="DateFrom"  ></telerik:GridBoundColumn>
                  <telerik:GridBoundColumn DataField="SubSetPoint" HeaderText="SetPoint" ></telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="DateTo" HeaderText="DateTo" ></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Remove" >
                                <ItemTemplate>
                                    <asp:Button CssClass="TableBtn" ID="Button3" runat="server" Text="Remove" CommandName="Remove" CommandArgument="<%# Container.ItemIndex %>" OnCommand="btnRemove" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
 
                 </Columns>
                 </MasterTableView>
             </telerik:RadGrid>

    
         </div>
        <br /><br />
        <div style="text-align: center; display:flex; gap:40px; justify-content:center; align-items:center;"><%--<asp:Label ID="label1"  runat ="server" >End Date/Time : </asp:Label><telerik:RadDateTimePicker Visible="false" ID="RadDateTimePicker2" runat="server" AutoPostBack="true" Width="250" Height="35" DateInput-DateFormat="yyyy-MM-dd HH:mm"></telerik:RadDateTimePicker>--%> <asp:Button ID="Insert" CssClass="CMR" runat="server" Text="Insert" OnClick="Insert_Click1" Visible="false"/></div>
    
        </div>
    </form>
</body>
</html>
