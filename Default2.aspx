﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">  
    <title></title>  
    <link href="Content/themes/base/jquery-ui.min.css" rel="stylesheet" />  
    <link href="Content/themes/base/base.css" rel="stylesheet" />  
    <link href="Content/themes/base/autocomplete.css" rel="stylesheet" />  
    <script src="Scripts/jquery-1.12.4.min.js"></script>  
    <script src="Scripts/jquery-ui-1.12.1.min.js"></script>  
    <script type="text/javascript">
      $(document).ready(function () {
        InitAutoCompleteEmployee();
      });

      function InitAutoCompleteEmployee() {
        $.widget("custom.combobox", {
          _create: function () {
            this.wrapper = $("<span>")
            .addClass("custom-combobox")
            .insertAfter(this.element);
            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
          },
          _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
            value = selected.val() ? selected.text() : "";
            this.input = $("<input>")
            .appendTo(this.wrapper)
            .val(value)
            .attr("title", "")
            .attr("style", "width:200px")
            .attr("id", "txtEmp")
            .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
            .autocomplete({
              delay: 0,
              //autoFocus: true,  
              source: function (request, response) {
                $.ajax({
                  type: "POST",
                  url: '<%=ResolveUrl("~/WebForm69.aspx/GetEmployeeDataSample") %>',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: "{ 'SearchParam': '" + $("#txtEmp").val() + "'}",
                                success: function (data) {
                                  var items = [];
                                  $.each(data.d, function (key, val) {
                                    var item = {
                                      label: val.EmpName,
                                      value: val.EmpId,
                                      id: val.EmpId,
                                      costFactor: val.EmpId
                                    };
                                    items.push(item);
                                  });
                                  source = items;
                                  response(items);

                                },
                                error: function (xhr) { debugger; }
                            });
                        },
                      select: function (event, ui) {
                        event.preventDefault();
                        $("[id$=txtEmp]").val(ui.item.label)
                      },
                      focus: function (event, ui) {
                        event.preventDefault();
                        $("[id$=txtEmp]").val(ui.item.label);
                      },
                    })
                    .tooltip({
                      tooltipClass: "ui-state-highlight"
                    });
                  this._on(this.input, {
                    autocompleteselect: function (event, ui) {
                      this._trigger("select", event, {
                        item: ui.item.label
                      });
                    },
                    autocompletechange: "_removeIfInvalid"
                  });
                },
              _createShowAllButton: function () {
                var input = this.input,
                wasOpen = false;
                $("<a>")
                .attr("tabIndex", -1)
                .attr("title", 'Select Employee')
                .tooltip()
                .appendTo(this.wrapper)
                .button({
                  icons: {
                    primary: "ui-icon-triangle-1-s"
                  },
                  text: false
                })
                .removeClass("ui-corner-all")
                .addClass("custom-combobox-toggle ui-corner-right")
                .mousedown(function () {
                  wasOpen = input.autocomplete("widget").is(":visible");
                })
                .click(function () {
                  input.focus();
                  if (wasOpen) {
                    return;
                  }
                  input.autocomplete("search", "*");
                });
              },
              _source: function (request, response) {
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                response(this.element.children("option").map(function () {
                  var text = $(this).text();
                  if (this.value && (!request.term || matcher.test(text)))
                    return {
                      label: text,
                      value: text,
                      option: this
                    };
                }));
              },
              _removeIfInvalid: function (event, ui) {
                if (ui.item) {
                  return;
                }
                var value = this.input.val(),
                valuevalueLowerCase = value.toLowerCase(),
                valid = false;
                this.element.children("option").each(function () {
                  if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                  }
                });
                if (valid) {
                  return;
                }
                this.input
                .val("")
                .attr("title", value + 'not found!')
                .tooltip("open");
                this.element.val("");
                $("[id$=hdnEmpId]").val("");
                this._delay(function () {
                  this.input.tooltip("close").attr("title", "");
                }, 2500);
                this.input.data("ui-autocomplete").term = "";
              },
              _destroy: function () {
                this.wrapper.remove();
                this.element.show();
              }
            });
                      $("#ddlEmployee").combobox();
                    }
    </script>  
</head>  
<body>  
    <form id="form1" runat="server">  
        <asp:DropDownList  
            runat="server"  
            ID="ddlEmployee"  
            ClientIDMode="Static"  
            Width="400" />  
        <asp:HiddenField  
            runat="server"  
            ClientIDMode="Static"  
            ID="hdnEmpId" />  
    </form>  
</body>  
</html>  