Imports MySql.Data.MySqlClient
Imports System.Globalization
Imports System.IO
Imports Microsoft.Win32

Public Class Form1
    Dim connnection As New MySqlConnection
    Dim user As String
    Dim initiation As Integer = 5

    Dim connectionStringOee As String

    'machine data monitoig (for hourly output monitoring)
    Dim DTMachineData As New DataTable
    Dim DTInformation As New DataTable
    Dim dT_machine_list As New DataTable

    'opc data
    Dim WithEvents daServerMgt As New Kepware.ClientAce.OpcDaClient.DaServerMgt
    Dim activeServerSubscriptionHandle As Integer
    Dim activeClientSubscriptionHandle As Integer

    Dim dt_counter_opc As New DataTable
    Dim dt_machine_status As New DataTable
    Dim dt_opc_function_list As New DataTable

    'to monitor product
    Dim product_time_monitoring As Integer = 20
    Dim shift_and_hour_monitoring As Integer = 4
    Dim update_monitoring As Integer = 20

    Public readwrtie As Integer
    Public strdata As New System.Text.StringBuilder(255)

    'selectio menu
    Dim selection_menu As String

    'auto closing
    Dim auto_closing As Boolean = True
    Dim timer_auto_closing As Integer = 10

#Region "Read Ini File"
    Private Declare Auto Function WritePrivateProfileString Lib "Kernel32" _
    (ByVal IpApplication As String, ByVal Ipkeyname As String,
    ByVal IpString As String, ByVal IpFileName As String) As Integer

    Private Declare Auto Function GetPrivateProfileString Lib "Kernel32" _
    (ByVal IpApplicationName As String, ByVal IpKeyName As String,
    ByVal IpDefault As String, ByVal IPReturnedString As System.Text.StringBuilder,
    ByVal nsize As Integer, ByVal IpFileName As String) As Integer


    Function get_system_setting(ByVal system_name As String)
        GetPrivateProfileString("system", system_name, "", strdata, 100, Application.StartupPath & "\setting.ini")

        If strdata.ToString = "" Then
            Return 0
        Else
            Return strdata.ToString
        End If
    End Function

    Function get_opc_text(ByVal opc_server As String, ByVal opc_text As String)
        GetPrivateProfileString(opc_server, opc_text, "", strdata, 100, Application.StartupPath & "\setting.ini")

        If strdata.ToString = "" Then
            Return 0
        Else
            Return strdata.ToString
        End If
    End Function

#End Region


#Region "loading Setting"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timerinitiation.Start()
    End Sub

    Private Sub Timerinitiation_Tick(sender As Object, e As EventArgs) Handles Timerinitiation.Tick
        initiation -= 1
        lbl_title.Text = initiation
        If initiation < 0 Then
            Timerinitiation.Stop()
            settingserverOEE()
            MakeDTInformation()
            MakeDTData()
            make_machine_list()
            getMachineList()
            view_machine_data_hourly(0)
            changeColorSelectedMachine(1)

            make_opc_list_function()
            make_counter_data()
            make_machine_status_data()

            find_counter_setting()
            find_machine_status_setting()

            'opc monitoring
            connect_opc_server()
            Subscribe_opc()

            TimerMonitoring.Start()
        End If


    End Sub

    Private Sub settingserverOEE()
        Dim Server As String = get_system_setting("server_name")
        Dim database As String = get_system_setting("server_database")
        Dim database_user As String = get_system_setting("server_uid")
        Dim database_password As String = get_system_setting("server_pasword")

        connectionStringOee = "Server=" & Server & ";Port=3306;Database=" & database & ";Uid= oee_user;Pwd=oee_user;Convert Zero Datetime=True"
    End Sub

    Private Sub make_machine_list()
        dT_machine_list.Columns.Add("id", GetType(String))
        dT_machine_list.Columns.Add("machine_code", GetType(String))
    End Sub

    Private Sub MakeDTInformation()
        DTInformation.Columns.Add("Data", GetType(String))
        DTInformation.Columns.Add("Information", GetType(String))
        DGVInformation.DataSource = DTInformation

        For i = 0 To DGVInformation.Columns.Count - 1
            DGVInformation.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
        Next i
    End Sub

    Private Sub MakeDTData()
        DTMachineData.Columns.Add("Machine Code", GetType(String))
        DTMachineData.Columns.Add("Nama Machine", GetType(String))
        DTMachineData.Columns.Add("Day Format", GetType(String))
        DTMachineData.Columns.Add("Date", GetType(String))
        DTMachineData.Columns.Add("Shift", GetType(String))
        DTMachineData.Columns.Add("shift Number", GetType(String))
        DTMachineData.Columns.Add("Shift Work", GetType(String))
        DTMachineData.Columns.Add("Shift Start", GetType(String))
        DTMachineData.Columns.Add("Shift End", GetType(String))
        DTMachineData.Columns.Add("Shift Code", GetType(String))
        DTMachineData.Columns.Add("Current Hour", GetType(String))
        DTMachineData.Columns.Add("Shift Hour Code", GetType(String))
        DTMachineData.Columns.Add("Hourly Record ID", GetType(String))
        DTMachineData.Columns.Add("Start Hour", GetType(String))
        DTMachineData.Columns.Add("End Hour", GetType(String))
        DTMachineData.Columns.Add("Product", GetType(String))
        DTMachineData.Columns.Add("Product Description", GetType(String))
        DTMachineData.Columns.Add("CT Target", GetType(String))
        DTMachineData.Columns.Add("Good Output", GetType(String))
        DTMachineData.Columns.Add("Reject", GetType(String))
        DTMachineData.Columns.Add("CT Average", GetType(String))
        DTMachineData.Columns.Add("CT Min", GetType(String))
        DTMachineData.Columns.Add("CT Max", GetType(String))
        DTMachineData.Columns.Add("Last Sinyal Output", GetType(String))
        DTMachineData.Columns.Add("Plan Output", GetType(String))
        DTMachineData.Columns.Add("Available Time", GetType(String))
        DTMachineData.Columns.Add("Loading Time", GetType(String))
        DTMachineData.Columns.Add("Plan DT", GetType(String))
        DTMachineData.Columns.Add("Downtime", GetType(String))
        DTMachineData.Columns.Add("Machine Status", GetType(String))
        DTMachineData.Columns.Add("Machine Status Category", GetType(String))
        DTMachineData.Columns.Add("Minor Stoppage", GetType(String))
        DTMachineData.Columns.Add("Number Minor Stoppage", GetType(String))
        DTMachineData.Columns.Add("Speed Loss", GetType(String))
        DTMachineData.Columns.Add("Number Speed Loss", GetType(String))
        DTMachineData.Columns.Add("Current Ct", GetType(String))
        DTMachineData.Columns.Add("Temp Actual Output", GetType(String))
        DTMachineData.Columns.Add("Temp CT Average", GetType(String))
        DTMachineData.Columns.Add("Temp CT Min", GetType(String))
        DTMachineData.Columns.Add("Temp CT Max", GetType(String))
        DTMachineData.Columns.Add("Temp Downtime", GetType(String))
        DTMachineData.Columns.Add("Temp Speed Loss", GetType(String))
        DTMachineData.Columns.Add("Temp Number Speed Loss", GetType(String))
        DTMachineData.Columns.Add("Temp Minor Stoppage", GetType(String))
        DTMachineData.Columns.Add("Temp Number Minor Stoppage", GetType(String))
        DTMachineData.Columns.Add("number product per cycle", GetType(String))
        DTMachineData.Columns.Add("number waiting count DT", GetType(String))
        DTMachineData.Columns.Add("number waiting count Speed Loss", GetType(String))
        DTMachineData.Columns.Add("number waiting count Minor_stoppage", GetType(String))
        DTMachineData.Columns.Add("Last Upload to server", GetType(String))
    End Sub

    Private Sub make_opc_list_function()
        dt_opc_function_list.Columns.Add("id", GetType(Double))
        dt_opc_function_list.Columns.Add("machine_code", GetType(String))

        dt_opc_function_list.Rows.Add(0, "Counter")
        dt_opc_function_list.Rows.Add(1, "Machine Status")
    End Sub

    Private Sub make_counter_data()
        dt_counter_opc.Columns.Add("no", GetType(String))
        dt_counter_opc.Columns.Add("counter", GetType(String))
        dt_counter_opc.Columns.Add("number", GetType(String))
        dt_counter_opc.Columns.Add("opc text", GetType(String))
        dt_counter_opc.Columns.Add("opc value", GetType(String))
        dt_counter_opc.Columns.Add("opc status", GetType(String))
        dt_counter_opc.Columns.Add("counter update", GetType(Double))
        dt_counter_opc.Columns.Add("machine id", GetType(String))
        dt_counter_opc.Columns.Add("machine list number", GetType(Double))
        dt_counter_opc.Columns.Add("counter id", GetType(String))
        dt_counter_opc.Columns.Add("counter name", GetType(String))
        dt_counter_opc.Columns.Add("counter type", GetType(String))
        dt_counter_opc.Columns.Add("reject record id", GetType(String))
    End Sub

    Private Sub make_machine_status_data()
        dt_machine_status.Columns.Add("No", GetType(String))
        dt_machine_status.Columns.Add("id", GetType(String))
        dt_machine_status.Columns.Add("machine id", GetType(String))
        dt_machine_status.Columns.Add("machine list number", GetType(String))
        dt_machine_status.Columns.Add("opc text1", GetType(String))
        dt_machine_status.Columns.Add("opc value1", GetType(String))
        dt_machine_status.Columns.Add("opc status1", GetType(String))
        dt_machine_status.Columns.Add("status machine number", GetType(String))
        dt_machine_status.Columns.Add("status machine id", GetType(String))
        dt_machine_status.Columns.Add("status text", GetType(String))
        dt_machine_status.Columns.Add("status record id", GetType(String))
        dt_machine_status.Columns.Add("status record start", GetType(String))
    End Sub

    Private Sub getMachineList()
        Dim select_terminal_assignment As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_user As New DataTable
        Dim data_machine_list As New DataTable
        Dim query_string As String

        user = get_system_setting("user") 'to get name user from setting database

        Dim machine_id As String
        Dim machine_description As String
        Dim day_format_id As Double
        Dim date_work As String
        Dim shift_id As String
        Dim shift_name As String
        Dim shift_number As Double
        Dim shift_work As String
        Dim shift_start As String
        Dim shift_end As String
        Dim shift_code As String
        Dim datecode As String
        Dim datenow As DateTime = DateTime.Now
        Dim current_hour As String = datenow.ToString("HH") & ":00"
        Dim shift_hour_records_id As String

        'get hourly record id
        Dim select_hour_output As New MySqlCommand
        Dim myAdapterselect_hour_output As New MySqlDataAdapter
        Dim data_shift_record As New DataTable
        Dim query_string_select_hour_output As String
        Dim shift_hour_output_id As String = ""
        Dim reject As Double = 0

        'get product
        Dim select_product As New MySqlCommand
        Dim myAdapterselect_product As New MySqlDataAdapter
        Dim data_product_paramater As New DataTable

        selection_menu = "Machine Status"

        DTMachineData.Rows.Clear()

        data_machine_list.Clear()

        connnection.ConnectionString = connectionStringOee

        Try
            connnection.Open()

            'getMachineList machine list base on user id
            query_string = "Select * FROM terminal_assignment INNER JOIN  machine On machine.id = terminal_assignment.machine_id " &
                        "Where terminal_id = '" & user & "' ORDER BY position"
            select_terminal_assignment.Connection = connnection
            select_terminal_assignment.CommandText = query_string

            myAdapter.SelectCommand = select_terminal_assignment


            myAdapter.Fill(data_machine_list)

            'Setting to show the machine group box
            show_button_list(data_machine_list.Rows().Count)


            'change name of button
            For i = 0 To data_machine_list.Rows().Count - 1
                Dim no_product_in_hour As Double = 0
                Dim hourly_records_id As String = ""
                Dim start_time As String = ""
                Dim end_time As String = ""
                Dim product As String = ""
                Dim product_description As String = ""
                Dim ct_target As String = 0
                Dim actual_output As String = 0
                Dim ct_average As String = 0
                Dim ct_minimum As String = 0
                Dim ct_maximum As String = 0
                Dim speed_loss_time As String = 0
                Dim speed_loss_number As String = 0
                Dim minor_stoppage_time As String = 0
                Dim minor_stoppage_number As String = 0
                Dim start_time_date As New DateTime
                Dim end_time_date As New DateTime
                Dim plan_output As Double = 0
                Dim available_time As Double = 0
                Dim loading_time As Double = 0
                Dim downtime As Double = 0
                Dim plan_downtime As Double = 0
                Dim machine_status As String = ""
                Dim status_category As String = ""


                'Get machine data
                machine_id = data_machine_list.Rows(i).Item("machine_id")
                machine_description = data_machine_list.Rows(i).Item("description")
                day_format_id = data_machine_list.Rows(i).Item("day_format_id")

                'Get shift data for every machine on region shift detection, check and analysis and put on tuple
                'read record shift
                Dim tMachineShift As Tuple(Of String, String, Double, String, String, String, String) = getMachineShift(machine_id)
                date_work = tMachineShift.Item1
                shift_name = tMachineShift.Item2
                shift_number = tMachineShift.Item3
                shift_work = tMachineShift.Item4
                shift_start = tMachineShift.Item5
                shift_end = tMachineShift.Item6
                shift_id = tMachineShift.Item7


                'get shift code from function
                If get_shift_record_id(machine_id, date_work, shift_number).ToString = "" Then 'not yet have recort for specific machine, date and shift number
                    datecode = Mid(date_work, 3, 2) & Mid(date_work, 6, 2) & Mid(date_work, 9, 2)
                    shift_code = machine_id & datecode & shift_number
                    create_shift_records(shift_code, machine_id, date_work, shift_number, shift_work, shift_start, shift_end, shift_name, shift_id, user)
                Else
                    shift_code = get_shift_record_id(machine_id, date_work, shift_number)
                End If

                'Get Shift hour id Records 
                shift_hour_records_id = get_shift_hour_code(shift_code, current_hour)

                'Get No product on shift hour records
                no_product_in_hour = get_no_product_in_hour(shift_hour_records_id)

                'get hour output id from function
                query_string_select_hour_output = "Select * FROM hourly_output_data left join product on hourly_output_data.product_id = product.id " &
                        "Where 	shift_record_hour_id = '" & shift_hour_records_id & "'"
                select_hour_output.Connection = connnection
                select_hour_output.CommandText = query_string_select_hour_output

                myAdapterselect_hour_output.SelectCommand = select_hour_output

                myAdapterselect_hour_output.Fill(data_shift_record)

                If data_shift_record.Rows().Count > 0 Then
                    hourly_records_id = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("id").ToString
                    start_time_date = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("start_time").ToString
                    start_time = start_time_date.ToString("yyyy/MM/dd HH:mm:ss")
                    end_time_date = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("end_time").ToString
                    end_time = end_time_date.ToString("yyyy/MM/dd HH:mm:ss")
                    plan_output = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("plan_output")
                    product = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("product_id").ToString
                    product_description = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("description").ToString
                    ct_target = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("ct_target")
                    actual_output = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("actual_output")
                    reject = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("reject_pcs")
                    available_time = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("available_time")
                    loading_time = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("loading_time")
                    plan_downtime = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("plan_downtime")
                    downtime = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("downtime")
                    ct_average = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("ct_average")
                    ct_minimum = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("ct_minimum")
                    ct_maximum = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("ct_maximum")
                    speed_loss_time = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("speed_loss_time")
                    speed_loss_number = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("speed_loss_number")
                    minor_stoppage_time = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("minor_stoppage_time")
                    minor_stoppage_number = data_shift_record.Rows(data_shift_record.Rows().Count - 1).Item("minor_stoppage_number")
                End If

                If hourly_records_id = "" Then 'not yet hae recort for specific machine, date and shift number
                    no_product_in_hour += 1
                    hourly_records_id = shift_hour_records_id & String.Format("{0:000#}", no_product_in_hour)
                    start_time = datenow.ToString("yyyy/MM/dd HH:mm:ss")
                    Dim next_time As DateTime = DateTime.Now.AddHours(1)
                    end_time = next_time.ToString("yyyy/MM/dd HH:00:00")
                    Dim end_time_date_time As DateTime = end_time
                    Dim duration As TimeSpan = end_time_date_time.Subtract(start_time)
                    available_time = duration.TotalSeconds
                    loading_time = duration.TotalSeconds

                    If Not (ct_target = "" Or ct_target = 0) Then
                        plan_output = loading_time / ct_target
                    End If

                    Dim query_string_paramater As String
                    'get hour output id from function
                    query_string_paramater = "Select * FROM parameter_machine left join product on parameter_machine.product_id = product.id " &
                        "Where 	machine_id = '" & machine_id & "'"
                    select_product.Connection = connnection
                    select_product.CommandText = query_string_paramater

                    myAdapterselect_product.SelectCommand = select_product
                    data_product_paramater.Clear()
                    myAdapterselect_product.Fill(data_product_paramater)

                    If data_product_paramater.Rows.Count = 1 Then
                        product = data_product_paramater.Rows(0).Item("product_id").ToString
                        ct_target = data_product_paramater.Rows(0).Item("ct_target").ToString
                        product_description = data_product_paramater.Rows(0).Item("description").ToString
                    End If

                    create_hourly_output_records(machine_id, product, shift_code, shift_hour_records_id, ct_target, i)
                End If

                Dim last_signal As String = datenow.ToString("yyyy/MM/dd HH:mm:ss")

                'get current status machine from machine
                Dim MCommand_machine As New MySqlCommand
                Dim query_machine As String
                Dim data_status_machine As New DataTable
                query_machine = "Select * FROM machine_status_records " &
                                            "Where machine_id= '" & machine_id & "' and end_time is Null order by start_time Desc"
                data_status_machine.Clear()
                MCommand_machine.Connection = connnection
                MCommand_machine.CommandText = query_machine
                myAdapter.SelectCommand = MCommand_machine
                myAdapter.Fill(data_status_machine)

                If data_status_machine.Rows().Count > 0 Then
                    machine_status = data_status_machine.Rows(0).Item("status_text_name")
                    status_category = data_status_machine.Rows(0).Item("status_category_name")
                End If

                'Add data into machine
                DTMachineData.Rows.Add(machine_id, machine_description, day_format_id, date_work, shift_name, shift_number, shift_work, shift_start, shift_end, shift_code,
                                    current_hour, shift_hour_records_id, hourly_records_id, start_time, end_time, product, product_description, ct_target,
                                    actual_output, reject, ct_average, ct_minimum, ct_maximum, last_signal, plan_output, available_time, loading_time,
                                    plan_downtime, downtime, machine_status, status_category)
                If i = 0 Then
                    BtnMachine1.Text = machine_id
                ElseIf i = 1 Then
                    BtnMachine2.Text = machine_id
                ElseIf i = 2 Then
                    BtnMachine3.Text = machine_id
                ElseIf i = 3 Then
                    BtnMachine4.Text = machine_id
                ElseIf i = 4 Then
                    BtnMachine5.Text = machine_id
                End If

                'to make list of machine to show
                dT_machine_list.Rows.Add(i, machine_id)
            Next

            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

#Region "Loading OPC Setting"
    Private Sub find_counter_setting()
        Dim indxto As Integer = 0
        Dim indxfrom As Integer = 0
        Dim allLinesto As List(Of String) = File.ReadAllLines(Application.StartupPath & "\setting.ini").ToList
        Dim allLinesfrom As List(Of String) = File.ReadAllLines(Application.StartupPath & "\setting.ini").ToList
        Dim start_number As Double 'to use for setting
        Dim number_counter As Double
        Dim counter As String
        Dim opc_text As String
        Dim count_counter As Double = 1

        'read data counter setting database
        Dim select_terminal_assignment As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_machine_reject As New DataTable
        Dim query_string As String

        user = get_system_setting("user") 'to get name user from setting database
        'connection to database
        connnection.ConnectionString = connectionStringOee

        Try
            connnection.Open()

            For Each line As String In allLinesto
                indxto = allLinesto.IndexOf(line)

                For Each item As String In allLinesfrom
                    indxfrom = allLinesto.IndexOf(item)
                    If line = item Then
                        If Mid(item, 1, 4) = "[OPC" Then
                            start_number = indxfrom
                        End If
                    End If
                Next

                For Each item As String In allLinesfrom
                    indxfrom = allLinesto.IndexOf(item)
                    If line = item Then
                        If Mid(item, 1, 2) = "c:" Then
                            If indxfrom > start_number Then
                                counter = Mid(item, 1, 7)
                                number_counter = Mid(item, 4, 4)
                                opc_text = get_opc_text("OPC1", counter)

                                'get data counter machine
                                query_string = "Select * FROM counter_assignment Where terminal_id = '" & user &
                                    "' and number =  '" & number_counter & "' ORDER BY id"
                                select_terminal_assignment.Connection = connnection
                                select_terminal_assignment.CommandText = query_string

                                myAdapter.SelectCommand = select_terminal_assignment

                                data_machine_reject.Clear()
                                myAdapter.Fill(data_machine_reject)

                                Dim machine_id As String = ""
                                Dim machine_list_number As String = ""
                                Dim counter_id As String = ""
                                Dim counter_name As String = ""
                                Dim counter_type As String = ""

                                If data_machine_reject.Rows().Count > 0 Then
                                    machine_id = data_machine_reject.Rows(0).Item("machine_id")
                                    counter_id = data_machine_reject.Rows(0).Item("reject_machine_id").ToString
                                    counter_name = data_machine_reject.Rows(0).Item("reject_name")
                                    counter_type = data_machine_reject.Rows(0).Item("counter_type")
                                End If

                                'get machine id on DT Machine List
                                Dim number_machine_list As String = get_machine_list_number(machine_id)

                                dt_counter_opc.Rows.Add(count_counter, counter, number_counter, opc_text, "", "", 0, machine_id,
                                                        number_machine_list, counter_id, counter_name, counter_type)
                                count_counter += 1

                            End If

                        End If
                    End If
                Next
            Next

            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

    Private Sub find_machine_status_setting()
        Dim indxto As Integer = 0
        Dim indxfrom As Integer = 0
        Dim allLinesto As List(Of String) = File.ReadAllLines(Application.StartupPath & "\setting.ini").ToList
        Dim allLinesfrom As List(Of String) = File.ReadAllLines(Application.StartupPath & "\setting.ini").ToList
        Dim start_number As Double 'to use for setting
        Dim machine_name As String
        Dim machine_status As String
        Dim opc_text As String
        Dim count_machine As Double = 1
        Dim machine_list_number As Double
        Dim numberEqual As Integer = 1

        For Each line As String In allLinesto
            indxto = allLinesto.IndexOf(line)

            For Each item As String In allLinesfrom
                indxfrom = allLinesto.IndexOf(item)
                If line = item Then
                    If Mid(item, 1, 4) = "[OPC" Then
                        start_number = indxfrom
                    End If
                End If
            Next

            For Each item As String In allLinesfrom
                indxfrom = allLinesto.IndexOf(item)

                If line = item Then
                    If Mid(item, 1, 5) = "MSTAT" Then
                        For i = 1 To item.Length
                            Dim CheckNumber As String = Mid(item, i, 1)
                            If CheckNumber = "=" Then
                                numberEqual = i - 1
                            End If
                        Next
                        If indxfrom > start_number Then

                            machine_status = Mid(item, 1, numberEqual)
                            machine_name = Mid(item, 6, numberEqual - 5)
                            opc_text = get_opc_text("OPC1", machine_status)
                            machine_list_number = get_machine_list_number(machine_name)
                            dt_machine_status.Rows.Add(count_machine, machine_status, machine_name, machine_list_number, opc_text)
                            count_machine += 1
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Function get_machine_list_number(ByVal machine_id As String) As String
        Dim number_machine_list As String = "No"
        For i As Integer = 0 To DTMachineData.Rows.Count - 1
            If machine_id = DTMachineData.Rows(i).Item(0) Then
                number_machine_list = i
            End If
        Next

        Return number_machine_list
    End Function
#End Region

#End Region

#Region "OPC Monitoring"

    Private Sub connect_opc_server()
        Dim url As String
        url = get_opc_text("OPC1", "hostname")

        ' The clientHandle allows us to give a meaningful reference number
        Dim clientHandle As Integer
        clientHandle = 1

        Dim connectInfo As New Kepware.ClientAce.OpcDaClient.ConnectInfo

        ' The connectinfo.ClientName parameter allows the client application
        connectInfo.ClientName = "OEE Client program"

        ' The LocalID member allows you to specify possible language options
        connectInfo.LocalId = "en"

        ' The KeepAliveTime member is the time interval, in ms
        connectInfo.KeepAliveTime = 60000

        ' The RetryAfterConnectionError tells the API to automatically
        ' try to reconnect after a connection loss. This is nice
        connectInfo.RetryAfterConnectionError = True

        ' The RetryInitialConnection tells the API to continue to try to establish an initial connection.
        connectInfo.RetryInitialConnection = False

        Dim connectFailed As Boolean
        connectFailed = False

        ' Call the Connect API method:
        Try
            daServerMgt.Connect(url, clientHandle, connectInfo, connectFailed)

            ' Update our connection status textbox
            ' MsgBox("Connection Succeeded")
        Catch ex As Exception
            MsgBox("Handled Connect exception. Reason: " & ex.Message)

            ' Make sure following code knows connection failed:
            connectFailed = True

            ' Update our connection status textbox
            MsgBox("Connection Failed")
        End Try
    End Sub

    Private Sub Subscribe_opc()
        Dim itemIndex As Integer = 0
        Dim clientSubscriptionHandle As Integer = 1
        Dim active As Boolean = True
        Dim updateRate As Integer = 500
        Dim deadBand As Single = 0
        Dim number_item As Integer = dt_counter_opc.Rows.Count + dt_machine_status.Rows.Count - 1

        Dim itemIdentifiers(number_item) As Kepware.ClientAce.OpcDaClient.ItemIdentifier

        For i As Integer = 0 To dt_counter_opc.Rows.Count - 1
            itemIdentifiers(itemIndex) = New Kepware.ClientAce.OpcDaClient.ItemIdentifier

            itemIdentifiers(itemIndex).ItemName = dt_counter_opc.Rows(i).Item(3)

            itemIdentifiers(itemIndex).ClientHandle = itemIndex

            itemIdentifiers(itemIndex).DataType = Type.GetType("System.string")
            itemIndex += 1
        Next

        For i As Integer = 0 To dt_machine_status.Rows.Count - 1
            itemIdentifiers(itemIndex) = New Kepware.ClientAce.OpcDaClient.ItemIdentifier

            itemIdentifiers(itemIndex).ItemName = dt_machine_status.Rows(i).Item(4)

            itemIdentifiers(itemIndex).ClientHandle = itemIndex

            itemIdentifiers(itemIndex).DataType = Type.GetType("System.string")
            itemIndex += 1
        Next

        Dim revisedUpdateRate As Integer


        Try
            daServerMgt.Subscribe(clientSubscriptionHandle, active, updateRate, revisedUpdateRate, deadBand, itemIdentifiers, activeServerSubscriptionHandle)

            activeClientSubscriptionHandle = clientSubscriptionHandle

            ' Check item result ID:
            For i = 0 To number_item
                If itemIdentifiers(i).ResultID.Succeeded = False Then

                    MsgBox("Failed to add item " & itemIdentifiers(i).ItemName & " to subscription")
                End If
            Next

        Catch ex As Exception
            MsgBox("Handled Subscribe exception. Reason: " & ex.Message)
        End Try
    End Sub

    Private Sub daServerMgt_DataChanged(ByVal clientSubscription As Integer, ByVal allQualitiesGood As Boolean, ByVal noErrors As Boolean, ByVal itemValues() As Kepware.ClientAce.OpcDaClient.ItemValueCallback) Handles daServerMgt.DataChanged
        My.Application.Log.WriteEntry("daServerMgt_DataChanged enter")

        ' We need to forward the callback to the main thread of the application if we access the GUI directly from the callback. 
        'It is recommended to do this even if the application is running in the back ground.
        BeginInvoke(New Kepware.ClientAce.OpcDaClient.DaServerMgt.DataChangedEventHandler(AddressOf DataChanged), New Object() {clientSubscription, allQualitiesGood, noErrors, itemValues})

        My.Application.Log.WriteEntry("daServerMgt_DataChanged exit")
    End Sub

    Private Sub DataChanged(ByVal clientSubscription As Integer, ByVal allQualitiesGood As Boolean, ByVal noErrors As Boolean, ByVal itemValues() As Kepware.ClientAce.OpcDaClient.ItemValueCallback)
        My.Application.Log.WriteEntry("DataChanged enter")

        If activeClientSubscriptionHandle = clientSubscription Then

            Dim itemValue As Kepware.ClientAce.OpcDaClient.ItemValueCallback

            For Each itemValue In itemValues
                Dim itemIndex As Integer
                itemIndex = itemValue.ClientHandle
                If itemValue.Quality.Name <> "OPC_QUALITY_NOT_CONNECTED" Then
                    Dim Value As String = itemValue.Value.ToString()
                    Dim Quality As String = itemValue.Quality.Name
                    If itemIndex < dt_counter_opc.Rows.Count Then
                        update_counter_opc(itemIndex, Value, Quality)
                    Else
                        update_machine_status_opc(itemIndex - dt_counter_opc.Rows.Count, Value, Quality)
                    End If
                End If
            Next
        End If

        My.Application.Log.WriteEntry("DataChanged exit")
    End Sub

    Private Sub DisconnectOPCServer()
        ' Call Disconnect API method:
        Try
            If daServerMgt.IsConnected Then
                daServerMgt.Disconnect()
            End If

        Catch ex As Exception
            MsgBox("Handled Disconnect exception. Reason: " & ex.Message)
        End Try
    End Sub

    Private Sub update_counter_opc(ByVal item_index As Double, ByVal item_value As Double, ByVal item_quality As String)
        Dim counter_type As String = dt_counter_opc.Rows(item_index).Item(11)
        Dim machine_list_number As Double = dt_counter_opc.Rows(item_index).Item(8)
        Dim machine_id As String = dt_counter_opc.Rows(item_index).Item(7)
        Dim reject_machine_id As String = dt_counter_opc.Rows(item_index).Item(9)
        Dim reject_name As String = dt_counter_opc.Rows(item_index).Item(10)

        Dim different As Double
        Dim counter_update As Double
        Dim reject_record_id As String = ""

        'hourly data
        Dim output_hourly As Double = DTMachineData.Rows(machine_list_number).Item("Good Output")
        Dim reject_hourly As Double = DTMachineData.Rows(machine_list_number).Item("Reject")

        If dt_counter_opc.Rows(item_index).Item(4) = "" Then 'loading first time opc
            If counter_type = "good" Then
                counter_update = DTMachineData.Rows(machine_list_number).Item("Good Output")
            ElseIf counter_type = "reject" Then
                Dim MCommand_counter As New MySqlCommand
                Dim myAdapter As New MySqlDataAdapter
                Dim data_reject As New DataTable
                Dim hourly_output_id As String = DTMachineData.Rows(machine_list_number).Item(12)

                connnection.ConnectionString = connectionStringOee
                Try
                    connnection.Open()

                    data_reject.Clear()
                    Dim query_string_reject As String
                    'get reject_id and counter
                    query_string_reject = "Select * FROM reject_record_hour " &
                                        "Where 	hourly_output_id = '" & hourly_output_id & "' and reject_machine_id = '" & reject_machine_id & "'"
                    MCommand_counter.Connection = connnection
                    MCommand_counter.CommandText = query_string_reject

                    myAdapter.SelectCommand = MCommand_counter
                    myAdapter.Fill(data_reject)

                    If data_reject.Rows().Count > 0 Then
                        counter_update = data_reject.Rows(0).Item("qty_pcs")
                        reject_record_id = data_reject.Rows(0).Item("id")
                    End If

                    'add reject record id
                    dt_counter_opc.Rows(item_index).Item(12) = reject_record_id

                    connnection.Close()
                Catch myerror As MySqlException
                    MessageBox.Show("Error Connecting to Database: " & myerror.Message)
                Finally
                    connnection.Dispose()
                End Try
            End If
        Else 'monitor change data after loading 
            Dim initial_value_opc As Double = dt_counter_opc.Rows(item_index).Item(4)
            Dim initial_counter_update As Double
            'calculate different
            If initial_value_opc <= item_value Then
                different = item_value - initial_value_opc
            Else
                different = item_value
            End If
            'update ct on hourly output
            update_ct(machine_list_number, different)

            If counter_type = "good" Then
                initial_counter_update = DTMachineData.Rows(machine_list_number).Item(18)
                counter_update = initial_counter_update + different
                DTMachineData.Rows(machine_list_number).Item(18) = counter_update

            ElseIf counter_type = "reject" Then
                reject_record_id = dt_counter_opc.Rows(item_index).Item(12)

                If reject_record_id = "" Then
                    Dim MCommand_counter As New MySqlCommand
                    Dim myAdapter As New MySqlDataAdapter
                    Dim data_reject As New DataTable
                    Dim hourly_output_id As String = DTMachineData.Rows(machine_list_number).Item(12)
                    Dim number_kind_reject_hourly As Double

                    connnection.ConnectionString = connectionStringOee
                    Try
                        connnection.Open()
                        data_reject.Clear()
                        Dim query_string_reject As String
                        'create reject id from hourly output id
                        query_string_reject = "Select * FROM reject_record_hour " &
                                        "Where 	hourly_output_id = '" & hourly_output_id & "'"
                        MCommand_counter.Connection = connnection
                        MCommand_counter.CommandText = query_string_reject

                        myAdapter.SelectCommand = MCommand_counter
                        myAdapter.Fill(data_reject)

                        number_kind_reject_hourly = data_reject.Rows().Count + 1
                        reject_record_id = hourly_output_id & String.Format("{0:000#}", number_kind_reject_hourly)

                        'insert new reject_hourly records
                        Dim insert_hourly_records As New MySqlCommand
                        Dim q_insert_hourly As String
                        Dim ct_target As Double = DTMachineData.Rows(machine_list_number).Item(17)
                        Dim product_id As String = DTMachineData.Rows(machine_list_number).Item(15)
                        Dim qty_time_reject As Double = ct_target * different
                        Dim datenow As DateTime = DateTime.Now
                        Dim time_update As String = datenow.ToString("yyyy-MM-dd HH:mm:ss")

                        q_insert_hourly = "INSERT INTO reject_record_hour (id, hourly_output_id, reject_machine_id, qty_pcs, qty_time," &
                            "product_id, machine_id, reject_text_id, reject_name,create_by,create_time) " &
                            "VALUES ('" & reject_record_id & "', '" & hourly_output_id & "', '" & reject_machine_id & "', '" & different &
                            "', '" & qty_time_reject & "', '" & product_id & "', '" & machine_id & "', '" & reject_machine_id & "', '" & reject_name &
                            "', 'OPC', '" & time_update & "')"
                        insert_hourly_records.Connection = connnection
                        insert_hourly_records.CommandText = q_insert_hourly
                        insert_hourly_records.ExecuteNonQuery()


                        initial_counter_update = dt_counter_opc.Rows(item_index).Item(6)
                        counter_update = initial_counter_update + different

                        'add reject hourly
                        reject_hourly += different
                        DTMachineData.Rows(machine_list_number).Item("Reject") = reject_hourly

                        'add reject record id
                        dt_counter_opc.Rows(item_index).Item(12) = reject_record_id

                        connnection.Close()
                    Catch myerror As MySqlException
                        MessageBox.Show("Error Connecting to Database: " & myerror.Message)
                    Finally
                        connnection.Dispose()
                    End Try
                Else
                    initial_counter_update = dt_counter_opc.Rows(item_index).Item(6)
                    counter_update = initial_counter_update + different
                    'add reject hourly
                    reject_hourly += different
                    DTMachineData.Rows(machine_list_number).Item(19) = reject_hourly
                End If

            End If
        End If

        dt_counter_opc.Rows(item_index).Item(4) = item_value
        dt_counter_opc.Rows(item_index).Item(5) = item_quality
        dt_counter_opc.Rows(item_index).Item(6) = counter_update
    End Sub

    Private Sub update_ct(ByVal machine_number_list As Double, ByVal output_number As Double)
        Dim start_date_time As DateTime = DTMachineData.Rows(machine_number_list).Item("Last Sinyal Output").ToString
        Dim end_time As DateTime = Now
        Dim duration As TimeSpan = end_time.Subtract(start_date_time)
        Dim good_output As Double = DTMachineData.Rows(machine_number_list).Item(18).ToString
        Dim reject As Double = DTMachineData.Rows(machine_number_list).Item(19).ToString
        Dim total_output As Double = good_output + reject

        Dim initial_ct_average As Double = DTMachineData.Rows(machine_number_list).Item(20).ToString
        Dim initial_ct_min As Double = DTMachineData.Rows(machine_number_list).Item(21).ToString
        Dim initial_ct_max As Double = DTMachineData.Rows(machine_number_list).Item(22).ToString


        Dim ct_average As Double
        Dim ct_minimum As Double
        Dim ct_maximum As Double
        Dim ct_sinyal As Double = duration.TotalSeconds / output_number

        'MsgBox(start_date_time.ToString("HH:mm:ss") & " - " & end_time.ToString("HH:mm:ss") & " - " & duration.TotalSeconds & " - " & ct_sinyal)
        ct_average = (total_output * initial_ct_average + duration.TotalSeconds) / (total_output + output_number)

        If initial_ct_min = 0 Then
            ct_minimum = ct_sinyal
        ElseIf initial_ct_min < ct_sinyal Then
            ct_minimum = initial_ct_min
        Else
            ct_minimum = ct_sinyal
        End If

        If initial_ct_max > ct_sinyal Then
            ct_maximum = initial_ct_max
        Else
            ct_maximum = ct_sinyal
        End If

        If output_number > 0 Then
            DTMachineData.Rows(machine_number_list).Item(20) = String.Format("{0:0.00}", ct_average)
            DTMachineData.Rows(machine_number_list).Item(21) = String.Format("{0:0.00}", ct_minimum)
            DTMachineData.Rows(machine_number_list).Item(22) = String.Format("{0:0.00}", ct_maximum)
            DTMachineData.Rows(machine_number_list).Item("Last Sinyal Output") = end_time.ToString("yyyy/MM/dd HH:mm:ss")
        End If

    End Sub

    Private Sub update_machine_status_opc(ByVal item_index As Double, ByVal item_value As Double, ByVal item_quality As String)
        Dim MCommand_mstat As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_status_machine As New DataTable
        Dim Machine_id As String = dt_machine_status.Rows(item_index).Item(2)
        Dim Machine_list_number As String = dt_machine_status.Rows(item_index).Item(3)

        'get data machine
        Dim MCommand_machine As New MySqlCommand
        Dim data_machine As New DataTable

        connnection.ConnectionString = connectionStringOee
        Try
            connnection.Open()
            Dim query_machine_status As String

            'get status machine 
            data_status_machine.Clear()
            query_machine_status = "Select * FROM status_machine " &
                                        "Where machine_id = '" & Machine_id & "' and number = '" & item_value & "'"

            MCommand_mstat.Connection = connnection
            MCommand_mstat.CommandText = query_machine_status

            myAdapter.SelectCommand = MCommand_mstat
            myAdapter.Fill(data_status_machine)

            If data_status_machine.Rows().Count > 0 Then
                Dim status_machine_id As String = data_status_machine.Rows(0).Item("id")

                'get current status machine from machine
                Dim query_machine As String
                query_machine = "Select * FROM machine_status_records " &
                                        "Where machine_id= '" & Machine_id & "' and end_time is Null order by start_time Desc"

                MCommand_machine.Connection = connnection
                MCommand_machine.CommandText = query_machine
                myAdapter.SelectCommand = MCommand_machine
                myAdapter.Fill(data_machine)

                If data_machine.Rows().Count = 0 Then
                    create_change_machine_status(item_index, item_value)
                Else
                    Dim current_machine_status_id As String = data_machine.Rows(0).Item("status_machine_id")
                    Dim machine_status_record_id As String = data_machine.Rows(0).Item("id")
                    Dim machine_status_record_start As String = data_machine.Rows(0).Item("start_time")

                    If current_machine_status_id = status_machine_id Then

                        'update machine and machine status record when machine status change
                    Else
                        update_machine_status(item_index, item_value)
                        create_change_machine_status(item_index, item_value)
                    End If
                End If
            Else
                dt_machine_status.Rows(item_index).Item(7) = "status not found"
            End If

            connnection.Close()

        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
        dt_machine_status.Rows(item_index).Item(5) = item_value
        dt_machine_status.Rows(item_index).Item(6) = item_quality
    End Sub

    Private Sub update_machine_status(ByVal item_index As Double, ByVal item_value As Double)
        Dim Machine_id As String = dt_machine_status.Rows(item_index).Item(2)
        Dim time_now As DateTime = Now
        Dim end_time As String = time_now.ToString("yyyy-MM-dd HH:mm:ss")

        'Sql Command
        Dim MCommand As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter

        'update machine status records
        Dim data_machine_status As New DataTable
        Dim query_machine_status As String

        data_machine_status.Clear()
        query_machine_status = "Select * FROM machine_status_records " &
                                    "Where machine_id = '" & Machine_id & "' and end_time is Null"

        MCommand.Connection = connnection
        MCommand.CommandText = query_machine_status
        myAdapter.SelectCommand = MCommand
        myAdapter.Fill(data_machine_status)

        If data_machine_status.Rows.Count > 0 Then
            For i As Integer = 0 To data_machine_status.Rows.Count - 1
                Dim Machine_status_records_id As String = data_machine_status.Rows(i).Item("id")
                Dim start_date_time As DateTime = data_machine_status.Rows(i).Item("start_time")

                Dim duration As TimeSpan = time_now.Subtract(start_date_time)

                Dim q_update = "UPDATE machine_status_records  SET end_time = '" & end_time & "', duration = '" & duration.TotalSeconds &
                           "' where id = '" & Machine_status_records_id & "'"
                MCommand.Connection = connnection
                MCommand.CommandText = q_update
                MCommand.ExecuteNonQuery()
            Next
        End If

        'update machine status Downtime records
        Dim data_machine_status_downtime As New DataTable
        Dim query_machine_status_downtime As String

        data_machine_status_downtime.Clear()
        query_machine_status_downtime = "Select * FROM machine_status_downtime_records " &
                                    "Where machine_id = '" & Machine_id & "' and end_time is Null"

        MCommand.Connection = connnection
        MCommand.CommandText = query_machine_status_downtime
        myAdapter.SelectCommand = MCommand
        myAdapter.Fill(data_machine_status_downtime)

        If data_machine_status_downtime.Rows.Count > 0 Then
            For i As Integer = 0 To data_machine_status_downtime.Rows.Count - 1
                Dim Machine_status_records_id As String = data_machine_status_downtime.Rows(i).Item("id")
                Dim start_date_time As DateTime = data_machine_status_downtime.Rows(i).Item("start_time")

                Dim duration As TimeSpan = time_now.Subtract(start_date_time)

                Dim q_update = "UPDATE machine_status_downtime_records  SET end_time = '" & end_time & "', duration = '" & duration.TotalSeconds &
                           "' where id = '" & Machine_status_records_id & "'"
                MCommand.Connection = connnection
                MCommand.CommandText = q_update
                MCommand.ExecuteNonQuery()
            Next
        End If

        'update machine status hourly Downtime records
        Dim data_machine_status_hourly_downtime As New DataTable
        Dim query_machine_status_hourly_downtime As String

        data_machine_status_hourly_downtime.Clear()
        query_machine_status_hourly_downtime = "Select * FROM machine_status_downtime_hourly_records " &
                                    "Where machine_id = '" & Machine_id & "' and end_time is Null"

        MCommand.Connection = connnection
        MCommand.CommandText = query_machine_status_hourly_downtime
        myAdapter.SelectCommand = MCommand
        myAdapter.Fill(data_machine_status_hourly_downtime)

        If data_machine_status_hourly_downtime.Rows.Count > 0 Then
            For i As Integer = 0 To data_machine_status_hourly_downtime.Rows.Count - 1
                Dim Machine_status_records_id As String = data_machine_status_hourly_downtime.Rows(i).Item("id")
                Dim start_date_time As DateTime = data_machine_status_hourly_downtime.Rows(i).Item("start_time")
                Dim downtime_category As String = data_machine_status_hourly_downtime.Rows(i).Item("status_category_name")
                Dim hourly_output_data_id As String = data_machine_status_hourly_downtime.Rows(i).Item("hourly_output_data_id")

                Dim duration As TimeSpan = time_now.Subtract(start_date_time)

                Dim q_update = "UPDATE machine_status_downtime_hourly_records  SET end_time = '" & end_time & "', duration = '" & duration.TotalSeconds &
                           "' where id = '" & Machine_status_records_id & "'"
                MCommand.Connection = connnection
                MCommand.CommandText = q_update
                MCommand.ExecuteNonQuery()

                'update machine status hourly Downtime records
                Dim data_hourly_output As New DataTable
                Dim query_hourly_output As String

                data_hourly_output.Clear()
                query_hourly_output = "Select * FROM hourly_output_data " &
                                    "Where id = '" & hourly_output_data_id & "'"

                MCommand.Connection = connnection
                MCommand.CommandText = query_hourly_output
                myAdapter.SelectCommand = MCommand
                myAdapter.Fill(data_hourly_output)

                Dim loading_time As Double = data_hourly_output.Rows(0).Item("loading_time")
                Dim plan_downtime As Double = data_hourly_output.Rows(0).Item("plan_downtime")
                Dim downtime As Double = data_hourly_output.Rows(0).Item("downtime")
                Dim ct_target As Double = data_hourly_output.Rows(0).Item("ct_target")
                Dim plan_output As Double = data_hourly_output.Rows(0).Item("plan_output")

                If downtime_category = "Downtime" Then
                    downtime += duration.TotalSeconds

                    Dim q_update_output = "UPDATE hourly_output_data  SET downtime = '" & downtime &
                           "' where id = '" & hourly_output_data_id & "'"
                    MCommand.Connection = connnection
                    MCommand.CommandText = q_update_output
                    MCommand.ExecuteNonQuery()
                Else
                    loading_time -= duration.TotalSeconds
                    plan_downtime += duration.TotalSeconds

                    If ct_target > 0 Then
                        plan_output = loading_time / ct_target
                    End If

                    Dim q_update_output = "UPDATE hourly_output_data  SET loading_time = '" & loading_time & "',plan_downtime = '" & plan_downtime &
                           "',plan_output = '" & plan_output & "' where id = '" & hourly_output_data_id & "'"
                    MCommand.Connection = connnection
                    MCommand.CommandText = q_update_output
                    MCommand.ExecuteNonQuery()
                End If
            Next
        End If
    End Sub

    Private Sub create_change_machine_status(ByVal item_index As Double, ByVal item_value As Double)
        Dim MCommand_mstat As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_status_machine As New DataTable
        Dim Machine_id As String = dt_machine_status.Rows(item_index).Item(2)
        Dim Machine_list_number As Double = dt_machine_status.Rows(item_index).Item(3)

        'get data machine
        Dim MCommand_machine As New MySqlCommand
        Dim data_machine As New DataTable

        Dim query_machine_status As String

        'get status machine 
        data_status_machine.Clear()
        query_machine_status = "Select * FROM status_machine " &
                                    "Where machine_id = '" & Machine_id & "' and number = '" & item_value & "'"

        MCommand_mstat.Connection = connnection
        MCommand_mstat.CommandText = query_machine_status

        myAdapter.SelectCommand = MCommand_mstat
        myAdapter.Fill(data_status_machine)
        Dim status_machine_id As String = data_status_machine.Rows(0).Item("id")
        Dim status_machine_number As String = data_status_machine.Rows(0).Item("number")
        Dim status_text_id As String = data_status_machine.Rows(0).Item("status_text_id")
        Dim status_text_name As String = data_status_machine.Rows(0).Item("status_text")
        Dim status_type_id As String = data_status_machine.Rows(0).Item("status_type_id")
        Dim status_type_name As String = data_status_machine.Rows(0).Item("status_type_description")
        Dim status_category_id As Double = data_status_machine.Rows(0).Item("status_category_id")
        Dim Status_category_name As String = data_status_machine.Rows(0).Item("status_category_description")

        Dim datenow As DateTime = DateTime.Now
        Dim start_time As String = datenow.ToString("yyyy-MM-dd HH:mm:ss")
        Dim year_record As String = datenow.ToString("yyyy")
        Dim month_record As String = datenow.ToString("yyyyMM")
        Dim day_record As String = datenow.ToString("yyMMdd")
        Dim shift_record As String = DTMachineData.Rows(Machine_list_number).Item("shift Number")
        'Calculate Week
        Dim dfi = DateTimeFormatInfo.CurrentInfo
        Dim calendar = dfi.Calendar
        Dim weekOfyear = calendar.GetWeekOfYear(datenow, dfi.CalendarWeekRule, DayOfWeek.Monday)
        Dim week_day As Double = datenow.DayOfWeek
        Dim week As String

        If week_day = 1 And datenow.ToString("HH") < "07" Then
            weekOfyear -= 1
        End If
        week = datenow.ToString("yy") & String.Format("{0:0#}", weekOfyear)

        'update record
        Dim insert_record As New MySqlCommand
        Dim q_insert_records As String

        'insert machine status records
        Dim ms_record_id As String = Machine_id & datenow.ToString("yyMMddHHmmssfff")

        q_insert_records = "INSERT INTO machine_status_records (id, machine_id, start_time, status_machine_id," &
                            "status_text_id, status_text_name, status_type_id, status_type_name," &
                            "status_category_id, status_category_name, create_time, create_by, " &
                            "year, month, day, week, shift)" &
                            "VALUES ('" & ms_record_id & "','" & Machine_id & "','" & start_time & "','" & status_machine_id &
                            "','" & status_text_id & "','" & status_text_name & "','" & status_type_id & "','" & status_type_name &
                            "','" & status_category_id & "','" & Status_category_name & "','" & start_time & "','" & user &
                            "','" & year_record & "','" & month_record & "','" & day_record & "','" & week & "','" & shift_record & "')"
        insert_record.Connection = connnection
        insert_record.CommandText = q_insert_records
        insert_record.ExecuteNonQuery()

        Dim q_update = "UPDATE machine SET 	status_machine_id = '" & status_machine_id & "', status_machine_number = '" & item_value &
                           "', status_machine_text = '" & status_text_name & "',machine_status_record_id = '" & ms_record_id & "' where id = '" & Machine_id & "'"
        MCommand_mstat.Connection = connnection
        MCommand_mstat.CommandText = q_update
        MCommand_mstat.ExecuteNonQuery()

        If Status_category_name <> "Run Production" Then

            'update record
            Dim insert_record_downtime As New MySqlCommand
            Dim q_insert_records_downtime As String

            'insert machine status downtime records
            Dim ms_record_id_downtime As String = Machine_id & datenow.ToString("yyMMddHHmmssfff")

            q_insert_records_downtime = "INSERT INTO machine_status_downtime_records (id, machine_status_records_id, machine_id, start_time, status_machine_id," &
                                "status_text_id, status_text_name, status_type_id, status_type_name," &
                                "status_category_id, status_category_name, create_time, create_by, " &
                                "year, month, day, week, shift)" &
                                "VALUES ('" & ms_record_id_downtime & "','" & ms_record_id & "','" & Machine_id & "','" & start_time & "','" & status_machine_id &
                                "','" & status_text_id & "','" & status_text_name & "','" & status_type_id & "','" & status_type_name &
                                "','" & status_category_id & "','" & Status_category_name & "','" & start_time & "','" & user &
                                "','" & year_record & "','" & month_record & "','" & day_record & "','" & week & "','" & shift_record & "')"
            insert_record_downtime.Connection = connnection
            insert_record_downtime.CommandText = q_insert_records_downtime
            insert_record_downtime.ExecuteNonQuery()

            'update record
            Dim insert_record_hourly_downtime As New MySqlCommand
            Dim q_insert_records_hourly_downtime As String

            'insert machine status hourly downtime records
            Dim hourly_output_data As String = DTMachineData.Rows(item_index).Item("Hourly Record ID")
            Dim ms_record_id_hourly_downtime As String = hourly_output_data & datenow.ToString("mmssfff")

            q_insert_records_hourly_downtime = "INSERT INTO machine_status_downtime_hourly_records (id,  machine_id, start_time, status_machine_id," &
                                "status_text_id, status_text_name, status_type_id, status_type_name," &
                                "status_category_id, status_category_name, create_time, create_by, " &
                                "year, month, day, week, shift," &
                                "machine_status_records_id, machine_status_downtime_record, hourly_output_data_id)" &
                                "VALUES ('" & ms_record_id_hourly_downtime & "','" & Machine_id & "','" & start_time & "','" & status_machine_id &
                                "','" & status_text_id & "','" & status_text_name & "','" & status_type_id & "','" & status_type_name &
                                "','" & status_category_id & "','" & Status_category_name & "','" & start_time & "','" & user &
                                "','" & year_record & "','" & month_record & "','" & day_record & "','" & week & "','" & shift_record &
                                "','" & ms_record_id & "','" & ms_record_id_downtime & "','" & hourly_output_data & "')"
            insert_record_hourly_downtime.Connection = connnection
            insert_record_hourly_downtime.CommandText = q_insert_records_hourly_downtime
            insert_record_hourly_downtime.ExecuteNonQuery()
        Else
            Dim end_time As DateTime = Now
            DTMachineData.Rows(Machine_list_number).Item("Last Sinyal Output") = end_time.ToString("yyyy/MM/dd HH:mm:ss")
        End If

        dt_machine_status.Rows(item_index).Item(7) = item_value 'status_machine_number
        dt_machine_status.Rows(item_index).Item(8) = status_machine_id 'status_machine_id
        dt_machine_status.Rows(item_index).Item(9) = status_text_name 'status_text_name
        dt_machine_status.Rows(item_index).Item(10) = ms_record_id 'machine_status_record_id
        dt_machine_status.Rows(item_index).Item(11) = start_time 'shift_record

        'update machine 
        DTMachineData.Rows(Machine_list_number).Item("Machine Status") = status_text_name
        DTMachineData.Rows(Machine_list_number).Item("Machine Status Category") = Status_category_name
    End Sub
#End Region

#Region "shift detection, check and analysis"
    'to get master machine data
    Function getMachineShift(ByVal machine_id As String) As Tuple(Of String, String, Double, String, String, String, String)
        Dim select_shift As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_machine_shift As New DataTable
        Dim query_string As String
        Dim working_date As String
        Dim shift_id As String
        Dim shift_name As String
        Dim shift_number As Double
        Dim shift_work As String
        Dim start_shift As String
        Dim end_shift As String
        Dim date_now As Date = Date.Now
        Dim date_yesterday As DateTime = DateTime.Today.AddDays(-1)
        Dim current_hour As String = date_now.ToString("HH:mm:ss")


        'read data terminal assignement

        query_string = "Select * FROM shift_day inner join machine on shift_day.day_format_id =  machine.day_format_id " &
                        "Where machine.id = '" & machine_id & "' ORDER BY shift_number"
        select_shift.Connection = connnection
        select_shift.CommandText = query_string

        myAdapter.SelectCommand = select_shift

        myAdapter.Fill(data_machine_shift)

        'change name of button
        For i = 0 To data_machine_shift.Rows().Count - 1
            shift_id = data_machine_shift.Rows(i).Item("id").ToString
            shift_name = data_machine_shift.Rows(i).Item("name").ToString
            shift_number = data_machine_shift.Rows(i).Item("shift_number").ToString
            shift_work = data_machine_shift.Rows(i).Item("time").ToString
            start_shift = data_machine_shift.Rows(i).Item("shift_start").ToString
            end_shift = data_machine_shift.Rows(i).Item("shift_end").ToString

            'MsgBox(day_format_id & " " & start_shift & " " & end_shift & " " & current_hour)
            If start_shift < end_shift Then
                working_date = date_now.ToString("yyyy-MM-dd")
                If current_hour >= start_shift And current_hour < end_shift Then
                    Return New Tuple(Of String, String, Double, String, String, String, String)(working_date, shift_name, shift_number, shift_work, start_shift, end_shift, shift_id)
                End If
            Else
                If current_hour < start_shift Or current_hour >= end_shift Then
                    If current_hour < start_shift Then
                        working_date = date_yesterday.ToString("yyyy-MM-dd")
                    Else
                        working_date = date_now.ToString("yyyy-MM-dd")
                    End If

                    Return New Tuple(Of String, String, Double, String, String, String, String)(working_date, shift_name, shift_number, shift_work, start_shift, end_shift, shift_id)
                End If
            End If
        Next
    End Function

    'to check the specific machine, date and shift number already have record or not
    Function get_shift_record_id(ByVal machine_id As String, ByVal date_record As String, ByVal number_shift As Double) As String
        Dim select_shift As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_shift_record As New DataTable
        Dim query_string As String
        Dim shift_record_id As String = ""


        'read record shift
        query_string = "Select * FROM shift_record Where machine_id = '" & machine_id & "' and date = '" & date_record & "' and shift_number = '" & number_shift & "'"
        select_shift.Connection = connnection
        select_shift.CommandText = query_string

        myAdapter.SelectCommand = select_shift

        myAdapter.Fill(data_shift_record)

        If data_shift_record.Rows().Count > 0 Then
            shift_record_id = data_shift_record.Rows(0).Item("id").ToString
            Return shift_record_id
        Else
            Return shift_record_id
        End If

    End Function

    'to create new data shift records and shift hour records
    Private Sub create_shift_records(ByVal shift_records_id As String, ByVal machine_id As String,
                                     ByVal date_work As String, ByVal shift_number As String,
                                     ByVal shift_time As String, ByVal shift_start As String,
                                     ByVal shift_end As String, ByVal shift_name As String,
                                     ByVal shift_day_id As String, ByVal create_by As String)

        Dim insert_shift_records As New MySqlCommand
        Dim q_insert_records As String
        Dim datenow As DateTime = DateTime.Now
        Dim create_time As String = datenow.ToString("yyyy-MM-dd HH:mm:ss")

        'hourly shift record declaration
        Dim select_shift_hour As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_shift_hour_record As New DataTable
        Dim query_string As String
        Dim hour_time As String
        Dim shift_hour_number As Double
        Dim shift_hour_id As String
        Dim shift_id As String

        'update shift hour
        Dim insert_shift_hour_records As New MySqlCommand
        Dim q_insert_hour_records As String

        'Calculate Week
        Dim dfi = DateTimeFormatInfo.CurrentInfo
        Dim calendar = dfi.Calendar
        Dim weekOfyear = calendar.GetWeekOfYear(datenow, dfi.CalendarWeekRule, DayOfWeek.Monday)
        Dim week_day As Double = datenow.DayOfWeek
        Dim week As String

        If week_day = 1 And datenow.ToString("HH") < "07" Then
            weekOfyear -= 1
        End If

        week = datenow.ToString("yy") & String.Format("{0:0#}", weekOfyear)


        '" &", shift_time, shift_start, shift_end,shift_name, shift_day_id, create_by,create_time
        ' , '" & machine_id & "'
        q_insert_records = "INSERT INTO shift_record (id, machine_id, date, shift_number " &
                            ", shift_time, shift_start, shift_end,shift_name," &
                            "shift_day_id, create_by,create_time, 	week)" &
                            "VALUES ('" & shift_records_id & "', '" & machine_id & "', '" & date_work & "', '" & shift_number &
                            "', '" & shift_time & "', '" & shift_start & "', '" & shift_end & "', '" & shift_name &
                            "', '" & shift_day_id & "', '" & create_by & "', '" & create_time & "', '" & week & "')"
        insert_shift_records.Connection = connnection
        insert_shift_records.CommandText = q_insert_records
        insert_shift_records.ExecuteNonQuery()

        'read record shift
        query_string = "Select * FROM shift_hour Where 	shift_day_id = '" & shift_day_id & "' ORDER BY shift_hour_number"
        select_shift_hour.Connection = connnection
        select_shift_hour.CommandText = query_string

        myAdapter.SelectCommand = select_shift_hour

        myAdapter.Fill(data_shift_hour_record)

        For i As Integer = 0 To data_shift_hour_record.Rows().Count - 1
            hour_time = data_shift_hour_record.Rows(i).Item("hour_time").ToString
            shift_hour_number = data_shift_hour_record.Rows(i).Item("shift_hour_number").ToString
            shift_hour_id = shift_records_id & String.Format("{0:0#}", shift_hour_number)
            shift_id = data_shift_hour_record.Rows(i).Item("id").ToString

            Dim datetime_hour_time As DateTime = date_work
            Dim hour_check As Double = Mid(hour_time, 1, 2)

            If shift_number = 3 And hour_check < 23 Then
                datetime_hour_time = datetime_hour_time.AddDays(1)
            End If

            Dim date_hour_time As String = datetime_hour_time.ToString("yyyy-MM-dd ") & hour_time & ":00"

            q_insert_hour_records = "INSERT INTO shift_record_hour (id, shift_record_id, shift_hour_number, hour_time " &
                           ", machine_id, Date, number_shift, time_shift, " &
                           "name_shift, shift_id, create_by, create_time, date_hour_time)" &
                           "VALUES ('" & shift_hour_id & "', '" & shift_records_id & "', '" & shift_hour_number & "', '" & hour_time &
                           "', '" & machine_id & "', '" & date_work & "', '" & shift_number & "', '" & shift_time &
                           "', '" & shift_name & "', '" & shift_id & "', '" & create_by & "', '" & create_time & "', '" & date_hour_time & "')"
            insert_shift_hour_records.Connection = connnection
            insert_shift_hour_records.CommandText = q_insert_hour_records
            insert_shift_hour_records.ExecuteNonQuery()
        Next

    End Sub

    'to get data shift_hour_records_id
    Function get_shift_hour_code(ByVal shift_record_id As String, ByVal hour_time As String)
        Dim select_hour_shift As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_shift_record As New DataTable
        Dim query_string As String
        Dim shift_record_hour_id As String = ""


        'read record shift
        query_string = "Select * FROM shift_record_hour Where shift_record_id = '" & shift_record_id & "' and hour_time LIKE '" & hour_time & "%' order by id DESC"

        select_hour_shift.Connection = connnection
        select_hour_shift.CommandText = query_string

        myAdapter.SelectCommand = select_hour_shift

        myAdapter.Fill(data_shift_record)

        If data_shift_record.Rows().Count > 0 Then
            shift_record_hour_id = data_shift_record.Rows(0).Item("id").ToString
            Return shift_record_hour_id
        Else
            Return shift_record_hour_id
        End If

    End Function

    Function get_no_product_in_hour(ByVal shift_record_hour_id As String)
        Dim select_hour_shift As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_shift_record As New DataTable
        Dim query_string As String



        'read record shift
        query_string = "Select * FROM hourly_output_data Where shift_record_hour_id LIKE '" & shift_record_hour_id & "%'"
        select_hour_shift.Connection = connnection
        select_hour_shift.CommandText = query_string

        myAdapter.SelectCommand = select_hour_shift

        myAdapter.Fill(data_shift_record)

        Return data_shift_record.Rows().Count

    End Function

    Private Sub create_hourly_output_records(ByVal machine_id As String, ByVal product_id As String,
                                             ByVal shift_record_id As String, ByVal shift_record_hour_id As String,
                                             ByVal ct_target As String, ByVal Machine_list_number As Double)
        Dim insert_hourly_records As New MySqlCommand
        Dim q_insert_hourly As String
        Dim datenow As DateTime = DateTime.Now
        Dim next_time As DateTime = DateTime.Now.AddHours(1)
        Dim start_time As String = datenow.ToString("yyyy-MM-dd HH:mm:ss")
        Dim end_time As String = next_time.ToString("yyyy-MM-dd HH:00:00")
        Dim end_time_date_time As DateTime = end_time
        Dim duration As TimeSpan = end_time_date_time.Subtract(start_time)
        Dim loading_time As Double = duration.TotalSeconds

        'Calculate Week
        Dim dfi = DateTimeFormatInfo.CurrentInfo
        Dim calendar = dfi.Calendar
        Dim weekOfyear = calendar.GetWeekOfYear(datenow, dfi.CalendarWeekRule, DayOfWeek.Monday)
        Dim week_day As Double = datenow.DayOfWeek
        Dim week As String
        Dim records_datetime As String = datenow.ToString("yyyy-MM-dd HH:mm:ss")
        Dim mont_code As String = datenow.ToString("yyyy-MM")
        Dim records_year As String = datenow.ToString("yyyy")
        Dim records_month As String = datenow.ToString("MM")
        Dim records_date As String = datenow.ToString("yyyy-MM-dd")
        Dim records_time As String = datenow.ToString("HH:mm:ss")

        Dim plan_output As Double = 0

        If Not (ct_target = "" Or ct_target = 0) Then
            plan_output = loading_time / ct_target
        End If

        If week_day = 1 And datenow.ToString("HH") < "07" Then
            weekOfyear -= 1
        End If

        'get current status machine from machine
        Dim MCommand As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim query_machine_hourly As String
        Dim data_status_machine_hourly As New DataTable
        query_machine_hourly = "Select * FROM hourly_output_data " &
                                    "Where machine_id= '" & machine_id & "' and shift_record_hour_id = '" & shift_record_hour_id & "'"
        data_status_machine_hourly.Clear()
        MCommand.Connection = connnection
        MCommand.CommandText = query_machine_hourly
        myAdapter.SelectCommand = MCommand
        myAdapter.Fill(data_status_machine_hourly)

        'Data For input hourly output data
        Dim no_product_in_hour As Double = data_status_machine_hourly.Rows().Count + 1
        Dim hourly_output_id As String = shift_record_hour_id & String.Format("{0:000#}", no_product_in_hour)

        week = datenow.ToString("yy") & String.Format("{0:0#}", weekOfyear)

        q_insert_hourly = "INSERT INTO hourly_output_data (id, machine_id, product_id, shift_records_id, shift_record_hour_id," &
                            "start_time, loading_time, 	week, datetime, end_time, available_time, plan_output,  " &
                            "month_code,  year, month, date, time, ct_target)" &
                            "VALUES ('" & hourly_output_id & "', '" & machine_id & "', '" & product_id & "', '" & shift_record_id & "', '" & shift_record_hour_id &
                            "', '" & start_time & "', '" & loading_time & "', '" & week & "', '" & records_datetime & "', '" & end_time & "', '" & loading_time & "', '" & plan_output &
                            "', '" & mont_code & "', '" & records_year & "', '" & records_month & "', '" & records_date & "', '" & records_time & "', '" & ct_target & "')"
        insert_hourly_records.Connection = connnection
        insert_hourly_records.CommandText = q_insert_hourly
        insert_hourly_records.ExecuteNonQuery()



        'get current status machine from machine
        Dim MCommand_machine As New MySqlCommand
        Dim query_machine As String
        Dim data_status_machine As New DataTable
        query_machine = "Select * FROM machine_status_records " &
                                    "Where machine_id= '" & machine_id & "' and end_time is Null order by start_time Desc"
        data_status_machine.Clear()
        MCommand_machine.Connection = connnection
        MCommand_machine.CommandText = query_machine
        myAdapter.SelectCommand = MCommand_machine
        myAdapter.Fill(data_status_machine)

        If data_status_machine.Rows().Count > 0 Then
            Dim status_machine_record_id As String = data_status_machine.Rows(0).Item("id")
            Dim status_text_id As String = data_status_machine.Rows(0).Item("status_text_id")
            Dim status_text_name As String = data_status_machine.Rows(0).Item("status_text_name")
            Dim status_type_id As String = data_status_machine.Rows(0).Item("status_type_id")
            Dim status_type_name As String = data_status_machine.Rows(0).Item("status_type_name")
            Dim status_category_id As Double = data_status_machine.Rows(0).Item("status_category_id")
            Dim Status_category_name As String = data_status_machine.Rows(0).Item("status_category_name")

            If Not (Status_category_name = "Run Production") Then
                'update machine status hourly Downtime records
                Dim data_machine_status_hourly_downtime As New DataTable
                Dim query_machine_status_hourly_downtime As String
                Dim time_now As DateTime = Now
                Dim end_time_record As String = time_now.ToString("yyyy-MM-dd HH:mm:ss")

                data_machine_status_hourly_downtime.Clear()
                query_machine_status_hourly_downtime = "Select * FROM machine_status_downtime_hourly_records " &
                                            "Where machine_id = '" & machine_id & "' and end_time is Null"

                MCommand.Connection = connnection
                MCommand.CommandText = query_machine_status_hourly_downtime
                myAdapter.SelectCommand = MCommand
                myAdapter.Fill(data_machine_status_hourly_downtime)

                'start update machine status hourly downtime
                If data_machine_status_hourly_downtime.Rows.Count > 0 Then
                    For j As Integer = 0 To data_machine_status_hourly_downtime.Rows.Count - 1
                        Dim Machine_status_records_id As String = data_machine_status_hourly_downtime.Rows(j).Item("id")
                        Dim start_date_time As DateTime = data_machine_status_hourly_downtime.Rows(j).Item("start_time")
                        Dim downtime_category As String = data_machine_status_hourly_downtime.Rows(j).Item("status_category_name")
                        Dim hourly_output_data_id As String = data_machine_status_hourly_downtime.Rows(j).Item("hourly_output_data_id")

                        Dim duration_records As TimeSpan = time_now.Subtract(start_date_time)

                        Dim q_update = "UPDATE machine_status_downtime_hourly_records  SET end_time = '" & end_time_record & "', duration = '" & duration_records.TotalSeconds &
                                   "' where id = '" & Machine_status_records_id & "'"
                        MCommand.Connection = connnection
                        MCommand.CommandText = q_update
                        MCommand.ExecuteNonQuery()

                        'update machine status hourly Downtime records
                        Dim data_hourly_output As New DataTable
                        Dim query_hourly_output As String

                        data_hourly_output.Clear()
                        query_hourly_output = "Select * FROM hourly_output_data " &
                                            "Where id = '" & hourly_output_data_id & "'"

                        MCommand.Connection = connnection
                        MCommand.CommandText = query_hourly_output
                        myAdapter.SelectCommand = MCommand
                        myAdapter.Fill(data_hourly_output)

                        Dim loading_time_records As Double = data_hourly_output.Rows(0).Item("loading_time")
                        Dim plan_downtime As Double = data_hourly_output.Rows(0).Item("plan_downtime")
                        Dim downtime_records As Double = data_hourly_output.Rows(0).Item("downtime")
                        Dim ct_target_records As Double = data_hourly_output.Rows(0).Item("ct_target")
                        Dim plan_output_records As Double = data_hourly_output.Rows(0).Item("plan_output")

                        If downtime_category = "Downtime" Then
                            downtime_records += duration_records.TotalSeconds

                            Dim q_update_output = "UPDATE hourly_output_data  SET downtime = '" & downtime_records &
                                   "' where id = '" & hourly_output_data_id & "'"
                            MCommand.Connection = connnection
                            MCommand.CommandText = q_update_output
                            MCommand.ExecuteNonQuery()
                        Else
                            loading_time_records -= duration_records.TotalSeconds
                            plan_downtime += duration_records.TotalSeconds

                            If ct_target > 0 Then
                                plan_output = loading_time_records / ct_target
                            End If

                            Dim q_update_output = "UPDATE hourly_output_data  SET loading_time = '" & loading_time_records & "',plan_downtime = '" & plan_downtime &
                                   "',plan_output = '" & plan_output & "' where id = '" & hourly_output_data_id & "'"
                            MCommand.Connection = connnection
                            MCommand.CommandText = q_update_output
                            MCommand.ExecuteNonQuery()
                        End If
                    Next
                End If

                'end update machine status hourly downtime
                Dim year_record As String = datenow.ToString("yyyy")
                Dim month_record As String = datenow.ToString("yyyyMM")
                Dim day_record As String = datenow.ToString("yyMMdd")
                Dim tMachineShift As Tuple(Of String, String, Double, String, String, String, String) = getMachineShift(machine_id)
                Dim shift_record As String = tMachineShift.Item3

                Dim insert_record_hourly_downtime As New MySqlCommand
                Dim q_insert_records_hourly_downtime As String

                'insert machine status hourly downtime records
                Dim hourly_output_data As String = hourly_output_id
                Dim ms_record_id_hourly_downtime As String = hourly_output_data & datenow.ToString("mmssfff")

                q_insert_records_hourly_downtime = "INSERT INTO machine_status_downtime_hourly_records (id,  machine_id, start_time, status_machine_id," &
                                    "status_text_id, status_text_name, status_type_id, status_type_name," &
                                    "status_category_id, status_category_name, create_time, create_by, " &
                                    "year, month, day, week, shift," &
                                    "machine_status_records_id, machine_status_downtime_record, hourly_output_data_id)" &
                                    "VALUES ('" & ms_record_id_hourly_downtime & "','" & machine_id & "','" & start_time & "','" & status_machine_record_id &
                                    "','" & status_text_id & "','" & status_text_name & "','" & status_type_id & "','" & status_type_name &
                                    "','" & status_category_id & "','" & Status_category_name & "','" & start_time & "','" & user &
                                    "','" & year_record & "','" & month_record & "','" & day_record & "','" & week & "','" & shift_record &
                                    "','" & status_machine_record_id & "','" & status_machine_record_id & "','" & hourly_output_data & "')"
                insert_record_hourly_downtime.Connection = connnection
                insert_record_hourly_downtime.CommandText = q_insert_records_hourly_downtime
                insert_record_hourly_downtime.ExecuteNonQuery()
            End If
        End If

        If DTMachineData.Rows().Count > Machine_list_number Then
            DTMachineData.Rows(Machine_list_number).Item("Current Hour") = DateTime.Now.ToString("HH:00")
            DTMachineData.Rows(Machine_list_number).Item("Shift Hour Code") = shift_record_hour_id
            DTMachineData.Rows(Machine_list_number).Item("Hourly Record ID") = hourly_output_id
            DTMachineData.Rows(Machine_list_number).Item("Start Hour") = DateTime.Now.ToString("HH:mm:ss")
            DTMachineData.Rows(Machine_list_number).Item("End Hour") = next_time.ToString("HH:00:00")
            DTMachineData.Rows(Machine_list_number).Item("Good Output") = 0
            DTMachineData.Rows(Machine_list_number).Item("Reject") = 0
            DTMachineData.Rows(Machine_list_number).Item("CT Average") = 0
            DTMachineData.Rows(Machine_list_number).Item("CT Min") = 0
            DTMachineData.Rows(Machine_list_number).Item("CT Max") = 0
        End If


        'update counter
        If dt_counter_opc.Rows().Count > 0 Then
            For i = 0 To dt_counter_opc.Rows().Count - 1
                dt_counter_opc.Rows(i).Item("counter update") = 0
                dt_counter_opc.Rows(i).Item("reject record id") = ""
            Next
        End If
    End Sub

#End Region

#Region "Selection Menu and viw management"
    Private Sub MesinStatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MesinStatusToolStripMenuItem.Click
        selection_menu = "Machine Status"
        lbl_title.Text = "Production Data Acquisition ( " & selection_menu & " )"

        Dim id As Double
        Dim name As String
        show_button_list(dT_machine_list.Rows.Count)
        For i As Integer = 0 To dT_machine_list.Rows.Count - 1
            id = dT_machine_list.Rows(i).Item(0)
            name = dT_machine_list.Rows(i).Item(1)

            If i = 0 Then
                BtnMachine1.Text = name
            ElseIf i = 1 Then
                BtnMachine2.Text = name
            ElseIf i = 2 Then
                BtnMachine3.Text = name
            ElseIf i = 3 Then
                BtnMachine4.Text = name
            ElseIf i = 4 Then
                BtnMachine5.Text = name
            End If
        Next

        view_machine_data_hourly(0)
        changeColorSelectedMachine(1)
    End Sub

    Private Sub OPCStatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OPCStatusToolStripMenuItem.Click
        selection_menu = "OPC Status"
        lbl_title.Text = "Production Data Acquisition ( " & selection_menu & " )"

        Dim id As Double
        Dim name As String
        show_button_list(dt_opc_function_list.Rows.Count)
        For i As Integer = 0 To dt_opc_function_list.Rows.Count - 1
            id = dt_opc_function_list.Rows(i).Item(0)
            name = dt_opc_function_list.Rows(i).Item(1)

            If i = 0 Then
                BtnMachine1.Text = name
            ElseIf i = 1 Then
                BtnMachine2.Text = name
            End If
        Next
        view_opc_data(0)
        changeColorSelectedMachine(1)
    End Sub

    Private Sub show_button_list(ByVal number As Double)
        If number < 1 Then
            GBMachine1.Visible = False
            GBMachine2.Visible = False
            GBMachine3.Visible = False
            GBMachine4.Visible = False
            GBMachine5.Visible = False
        ElseIf number <= 1 Then
            GBMachine1.Visible = True
            GBMachine2.Visible = False
            GBMachine3.Visible = False
            GBMachine4.Visible = False
            GBMachine5.Visible = False
        ElseIf number <= 2 Then
            GBMachine1.Visible = True
            GBMachine2.Visible = True
            GBMachine3.Visible = False
            GBMachine4.Visible = False
            GBMachine5.Visible = False
        ElseIf number <= 3 Then
            GBMachine1.Visible = True
            GBMachine2.Visible = True
            GBMachine3.Visible = True
            GBMachine4.Visible = False
            GBMachine5.Visible = False
        ElseIf number <= 4 Then
            GBMachine1.Visible = True
            GBMachine2.Visible = True
            GBMachine3.Visible = True
            GBMachine4.Visible = True
            GBMachine5.Visible = False
        ElseIf number >= 5 Then
            GBMachine1.Visible = True
            GBMachine2.Visible = True
            GBMachine3.Visible = True
            GBMachine4.Visible = True
            GBMachine5.Visible = True
        End If

    End Sub

    Private Sub view_data_button(ByVal number As Integer)
        If selection_menu = "Machine Status" Then
            view_machine_data_hourly(number)
        ElseIf selection_menu = "OPC Status" Then
            view_opc_data(number)
        End If
    End Sub

    Private Sub view_machine_data_hourly(ByVal number As Double)
        Dim i As Integer = 0
        Dim InformationData As String
        Dim MachineData As String

        DTInformation.Rows.Clear()

        For Each column As DataColumn In DTMachineData.Columns
            InformationData = column.ColumnName

            MachineData = DTMachineData.Rows(number).Item(i).ToString

            DTInformation.Rows.Add(InformationData, MachineData)
            i += 1
        Next

        DGVInformation.DataSource = DTInformation

        For i = 0 To DGVInformation.Columns.Count - 1
            DGVInformation.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
        Next i
    End Sub

    Private Sub view_opc_data(ByVal number As Double)
        Dim i As Integer = 0

        If number = 0 Then
            DGVInformation.DataSource = dt_counter_opc

            For i = 0 To DGVInformation.Columns.Count - 1
                DGVInformation.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
            Next i
        ElseIf number = 1 Then
            DGVInformation.DataSource = dt_machine_status


        End If
    End Sub

#Region "Selected Button"
    Private Sub BtnMachine1_Click(sender As Object, e As EventArgs) Handles BtnMachine1.Click
        changeColorSelectedMachine(1)
        view_data_button(0)
    End Sub

    Private Sub BtnMachine2_Click(sender As Object, e As EventArgs) Handles BtnMachine2.Click
        changeColorSelectedMachine(2)
        view_data_button(1)
    End Sub

    Private Sub BtnMachine3_Click(sender As Object, e As EventArgs) Handles BtnMachine3.Click
        changeColorSelectedMachine(3)
        view_data_button(2)
    End Sub

    Private Sub BtnMachine4_Click(sender As Object, e As EventArgs) Handles BtnMachine4.Click
        changeColorSelectedMachine(4)
        view_data_button(3)
    End Sub

    Private Sub BtnMachine5_Click(sender As Object, e As EventArgs) Handles BtnMachine5.Click
        changeColorSelectedMachine(5)
        view_data_button(4)
    End Sub

    Private Sub changeColorSelectedMachine(ByVal number)
        If number = 1 Then
            BtnMachine1.BackColor = Color.SkyBlue
            BtnMachine2.BackColor = Color.SlateGray
            BtnMachine3.BackColor = Color.SlateGray
            BtnMachine4.BackColor = Color.SlateGray
            BtnMachine5.BackColor = Color.SlateGray
        ElseIf number = 2 Then
            BtnMachine1.BackColor = Color.SlateGray
            BtnMachine2.BackColor = Color.SkyBlue
            BtnMachine3.BackColor = Color.SlateGray
            BtnMachine4.BackColor = Color.SlateGray
            BtnMachine5.BackColor = Color.SlateGray
        ElseIf number = 3 Then
            BtnMachine1.BackColor = Color.SlateGray
            BtnMachine2.BackColor = Color.SlateGray
            BtnMachine3.BackColor = Color.SkyBlue
            BtnMachine4.BackColor = Color.SlateGray
            BtnMachine5.BackColor = Color.SlateGray
        ElseIf number = 4 Then
            BtnMachine1.BackColor = Color.SlateGray
            BtnMachine2.BackColor = Color.SlateGray
            BtnMachine3.BackColor = Color.SlateGray
            BtnMachine4.BackColor = Color.SkyBlue
            BtnMachine5.BackColor = Color.SlateGray
        ElseIf number = 5 Then
            BtnMachine1.BackColor = Color.SlateGray
            BtnMachine2.BackColor = Color.SlateGray
            BtnMachine3.BackColor = Color.SlateGray
            BtnMachine4.BackColor = Color.SlateGray
            BtnMachine5.BackColor = Color.SkyBlue
        End If
    End Sub
#End Region

#End Region

#Region "Time Monitoring"
    Private Sub TimerMonitoring_Tick(sender As Object, e As EventArgs) Handles TimerMonitoring.Tick
        Dim DateNow As Date = Date.Now
        LblDateTime.Text = DateNow.ToString("dd MMMM yyyy HH:mm:ss")

        lbl_title.Text = "Production Data Acquisition ( " & selection_menu & " ) " & update_monitoring & " " &
                           shift_and_hour_monitoring & " auto closing " & auto_closing & " " & timer_auto_closing

        If update_monitoring <= 0 Then
            update_hourly_output_and_reject()
            update_monitoring = 20
        End If

        If product_time_monitoring <= 0 Then
            product_time_monitoring = 1
            'check_and_update_product()
        End If

        If shift_and_hour_monitoring <= 0 Then
            shift_and_hour_monitoring = 2
            check_shift()
            check_shift_hour_change()
        End If

        'auto closing program
        If timer_auto_closing <= 0 Then
            If auto_closing = True And ShowInTaskbar = True Then
                NotifyIcon1.Visible = True
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.BalloonTipTitle = "oee data acquisition"
                NotifyIcon1.BalloonTipText = "oee data acquisition"
                NotifyIcon1.ShowBalloonTip(10)
                Me.WindowState = FormWindowState.Minimized
                ShowInTaskbar = False
            End If
            timer_auto_closing = 10
        End If

        product_time_monitoring -= 1
        shift_and_hour_monitoring -= 1
        update_monitoring -= 1
        timer_auto_closing -= 1
    End Sub

    'to update hourly output and hourly riject base on DTmachine data and dt_opc_counter
    Private Sub update_hourly_output_and_reject()
        Dim myAdapter As New MySqlDataAdapter
        Dim data_reject As New DataTable
        connnection.ConnectionString = connectionStringOee
        Try
            connnection.Open()
            TimerMonitoring.Stop()

            For i As Integer = 0 To DTMachineData.Rows.Count - 1
                Dim MCommand_update_output As New MySqlCommand
                Dim q_update_output As String
                Dim hourly_output_id As String = DTMachineData.Rows(i).Item(12)
                Dim ct_target As Double = DTMachineData.Rows(i).Item(17)
                Dim good_output As Double = DTMachineData.Rows(i).Item(18)
                Dim reject As Double = DTMachineData.Rows(i).Item(19)
                Dim reject_time As Double = ct_target * reject

                Dim ct_average As Double = DTMachineData.Rows(i).Item(20)
                Dim ct_minimum As Double = DTMachineData.Rows(i).Item(21)
                Dim ct_maximum As Double = DTMachineData.Rows(i).Item(22)

                q_update_output = "UPDATE hourly_output_data  SET actual_output = '" & good_output & "', reject_pcs = '" & reject &
                            "',	reject_time = '" & reject_time & "', ct_average = '" & ct_average &
                            "', ct_minimum = '" & ct_minimum & "', ct_maximum = '" & ct_maximum &
                            "' where id = '" & hourly_output_id & "'"
                MCommand_update_output.Connection = connnection
                MCommand_update_output.CommandText = q_update_output
                MCommand_update_output.ExecuteNonQuery()
            Next

            For i As Integer = 0 To dt_counter_opc.Rows.Count - 1
                Dim MCommand_update_reject As New MySqlCommand
                Dim q_update_output As String
                Dim hourly_reject_id As String = dt_counter_opc.Rows(i).Item(12).ToString
                Dim machine_list_number As Double = dt_counter_opc.Rows(i).Item(8)
                Dim ct_target As Double = DTMachineData.Rows(machine_list_number).Item(17)
                Dim reject_qty As Double = dt_counter_opc.Rows(i).Item(6)
                Dim reject_time As Double = reject_qty * ct_target


                If hourly_reject_id <> "" Then
                    q_update_output = "UPDATE reject_record_hour  SET qty_pcs = '" & reject_qty & "', 	qty_time = '" & reject_time &
                           "' where id = '" & hourly_reject_id & "'"
                    MCommand_update_reject.Connection = connnection
                    MCommand_update_reject.CommandText = q_update_output
                    MCommand_update_reject.ExecuteNonQuery()
                End If
            Next

            TimerMonitoring.Start()
            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

    'monitoring hour change and update
    Private Sub check_shift_hour_change()
        Dim machine_id As String
        Dim current_hour As String
        Dim monitoring_hour As String = Date.Now.ToString("HH")

        Dim new_hour As Boolean = False

        For i As Integer = 0 To DTMachineData.Rows.Count - 1
            current_hour = Mid(DTMachineData.Rows(i).Item("Current Hour"), 1, 2)

            If current_hour <> monitoring_hour Then
                new_hour = True
                Exit For
            End If
        Next

        If new_hour Then
            update_hourly_output_and_reject()
            update_change_hour(monitoring_hour)
        End If
    End Sub

    Private Sub update_change_hour(ByVal new_hour_time As String)
        connnection.ConnectionString = connectionStringOee
        Try
            connnection.Open()
            TimerMonitoring.Stop()

            For i As Integer = 0 To DTMachineData.Rows.Count - 1
                Dim machine_id As String = DTMachineData.Rows(i).Item("Machine Code")
                Dim product_id As String = DTMachineData.Rows(i).Item("Product")
                Dim shift_record_id As String = DTMachineData.Rows(i).Item("Shift Code")
                Dim Shift_record_hour_id As String = get_shift_hour_code(shift_record_id, new_hour_time)
                Dim ct_target As String = DTMachineData.Rows(i).Item("CT Target")

                create_hourly_output_records(machine_id, product_id, shift_record_id, Shift_record_hour_id, ct_target, i)
            Next

            TimerMonitoring.Start()
            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

    Private Sub update_time_and_duration_last_hourly_output_id(ByVal hourly_record_id As String)
        Dim select_product As New MySqlCommand
        Dim update_record As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_trolley As New DataTable
        Dim query_string As String
        Dim query_update_record As String

        'update last hourly output
        query_string = "Select * FROM hourly_output_data where id = '" & hourly_record_id & "'"
        select_product.Connection = connnection
        select_product.CommandText = query_string
        myAdapter.SelectCommand = select_product
        myAdapter.Fill(data_trolley)

        Dim product_record As String = data_trolley.Rows(0).Item("product_id").ToString
        Dim start_date_time As DateTime = data_trolley.Rows(0).Item("datetime")

        Dim end_time As DateTime = Now
        Dim duration As TimeSpan = end_time.Subtract(start_date_time)

        query_update_record = "UPDATE hourly_output_data  SET end_time = '" & end_time.ToString("HH:mm:ss") &
                    "', duration = '" & duration.TotalSeconds &
                    "' where id = '" & hourly_record_id & "'"
        update_record.Connection = connnection
        update_record.CommandText = query_update_record
        update_record.ExecuteNonQuery()
    End Sub

    Private Sub check_shift()
        Dim number_start As Double
        Dim number_end As Double
        Dim number_current As Double
        Dim start_shift As String
        Dim end_shift As String
        Dim current_hour As String = Date.Now.ToString("HH:mm:ss")
        Dim machine_id As String
        Dim new_shift As Boolean = False

        For i As Integer = 0 To DTMachineData.Rows.Count - 1
            start_shift = DTMachineData.Rows(i).Item("Shift Start").ToString
            end_shift = DTMachineData.Rows(i).Item("Shift End").ToString
            machine_id = DTMachineData.Rows(i).Item("Machine Code").ToString
            number_current = Mid(current_hour, 1, 2)
            number_start = Mid(start_shift, 1, 2)
            number_end = Mid(end_shift, 1, 2)

            If number_start < number_end Then
                If Not (number_current >= number_start And number_current < number_end) Then
                    'MsgBox(" 1 " & start_shift & " " & end_shift & " " & current_hour & new_shift)
                    new_shift = True
                End If
            Else
                If Not (number_current >= number_start Or number_current < number_end) Then
                    new_shift = True
                    'MsgBox("2" & start_shift & " " & end_shift & " " & current_hour & new_shift)
                End If
            End If
        Next

        If new_shift Then
            TimerMonitoring.Stop()
            For i As Integer = 0 To DTMachineData.Rows.Count - 1
                start_shift = DTMachineData.Rows(i).Item("Shift Start").ToString
                end_shift = DTMachineData.Rows(i).Item("Shift End").ToString
                machine_id = DTMachineData.Rows(i).Item("Machine Code").ToString

                'create database record for shift and shift hour record when new shift
                create_new_shift(machine_id, i, current_hour)
            Next

            TimerMonitoring.Start()
        End If
    End Sub

    Private Sub create_new_shift(ByVal machine_id As String, ByVal machine_number As Integer, ByVal current_hour As String)
        Dim date_work As String
        Dim shift_id As String
        Dim shift_name As String
        Dim shift_number As Double
        Dim shift_work As String
        Dim shift_start As String
        Dim shift_end As String
        Dim shift_code As String
        Dim datecode As String

        connnection.ConnectionString = connectionStringOee
        Try
            connnection.Open()

            'Get shift data for every machine on region shift detection, check and analysis and put on tuple
            Dim tMachineShift As Tuple(Of String, String, Double, String, String, String, String) = getMachineShift(machine_id)
            date_work = tMachineShift.Item1
            shift_name = tMachineShift.Item2
            shift_number = tMachineShift.Item3
            shift_work = tMachineShift.Item4
            shift_start = tMachineShift.Item5
            shift_end = tMachineShift.Item6
            shift_id = tMachineShift.Item7

            'get shift code from function
            If get_shift_record_id(machine_id, date_work, shift_number).ToString = "" Then 'not yet have recort for specific machine, date and shift number
                datecode = Mid(date_work, 3, 2) & Mid(date_work, 6, 2) & Mid(date_work, 9, 2)
                shift_code = machine_id & datecode & shift_number
                create_shift_records(shift_code, machine_id, date_work, shift_number, shift_work, shift_start, shift_end, shift_name, shift_id, user)
            Else
                shift_code = get_shift_record_id(machine_id, date_work, shift_number)
            End If

            'MsgBox(date_work & " " & shift_number & " " & shift_code)
            'update data machine data into machine
            DTMachineData.Rows(machine_number).Item("Date") = date_work
            DTMachineData.Rows(machine_number).Item("Shift") = shift_name
            DTMachineData.Rows(machine_number).Item("shift Number") = shift_number
            DTMachineData.Rows(machine_number).Item("Shift Work") = shift_work
            DTMachineData.Rows(machine_number).Item("Shift Start") = shift_start
            DTMachineData.Rows(machine_number).Item("Shift End") = shift_end
            DTMachineData.Rows(machine_number).Item("Shift Code") = shift_code

            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

    Private Sub check_and_update_product()
        Dim machine_id As String
        Dim select_product As New MySqlCommand
        Dim delete_trigger As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_trolley As New DataTable
        Dim query_string As String
        Dim query_delete As String
        Dim id_trigger As String
        Dim product As String
        Dim ct_target As Double
        Dim description As String
        Dim number_product_per_cycle As Double
        Dim number_ct_count_dt As Double
        Dim number_ct_count_speed_loss As Double
        Dim number_ct_count_minor_stoppage As Double

        connnection.ConnectionString = connectionStringOee
        Try
            connnection.Open()

            For i As Integer = 0 To DTMachineData.Rows.Count - 1
                machine_id = DTMachineData.Rows(i).Item(0)

                query_string = "Select * FROM product_change_trigger where machine_id = '" & machine_id & "'  ORDER BY datetime_update"
                select_product.Connection = connnection
                select_product.CommandText = query_string
                myAdapter.SelectCommand = select_product

                myAdapter.Fill(data_trolley)

                If data_trolley.Rows().Count > 0 Then
                    id_trigger = data_trolley.Rows(0).Item("id")
                    product = data_trolley.Rows(0).Item("product_id")
                    ct_target = data_trolley.Rows(0).Item("ct_target")
                    description = data_trolley.Rows(0).Item("description")
                    number_product_per_cycle = data_trolley.Rows(0).Item("number_product_per_cycle")
                    number_ct_count_dt = data_trolley.Rows(0).Item("number_ct_count_dt")
                    number_ct_count_speed_loss = data_trolley.Rows(0).Item("number_ct_count_speed_loss")
                    number_ct_count_minor_stoppage = data_trolley.Rows(0).Item("number_ct_count_minor_stoppage")

                    query_delete = "delete from product_change_trigger where id = '" & id_trigger & "'"
                    delete_trigger.Connection = connnection
                    delete_trigger.CommandText = query_delete
                    delete_trigger.ExecuteNonQuery()

                    If product <> DTMachineData.Rows(i).Item(15).ToString Then
                        DTMachineData.Rows(i).Item(15) = product
                        DTMachineData.Rows(i).Item(16) = description
                        DTMachineData.Rows(i).Item(17) = ct_target
                        DTMachineData.Rows(i).Item(38) = number_product_per_cycle
                        DTMachineData.Rows(i).Item(39) = number_ct_count_dt
                        DTMachineData.Rows(i).Item(40) = number_ct_count_speed_loss
                        DTMachineData.Rows(i).Item(41) = number_ct_count_minor_stoppage
                        'update_hourly_record(i, product, ct_target, number_product_per_cycle, number_ct_count_dt, number_ct_count_speed_loss, number_ct_count_minor_stoppage)
                        view_machine_data_hourly(i)
                    End If
                End If
            Next

            connnection.Close()
        Catch myerror As MySqlException
            MessageBox.Show("Error Connecting to Database: " & myerror.Message)
        Finally
            connnection.Dispose()
        End Try
    End Sub

    Private Sub update_hourly_record(ByVal number_machine As String, ByVal product As String, ByVal ct_target As Double,
                                     ByVal number_product_per_cycle As Double, ByVal number_ct_count_dt As Double,
                                     ByVal number_ct_count_speed_loss As Double, ByVal number_ct_count_minor_stppage As Double)
        Dim select_product As New MySqlCommand
        Dim update_record As New MySqlCommand
        Dim myAdapter As New MySqlDataAdapter
        Dim data_trolley As New DataTable
        Dim query_string As String
        Dim query_update_record As String
        Dim hourly_record_id As String = DTMachineData.Rows(number_machine).Item(12).ToString
        Dim shift_id As String = DTMachineData.Rows(number_machine).Item(8).ToString
        Dim shift_hour_id As String = DTMachineData.Rows(number_machine).Item(10).ToString
        Dim machine_id As String = DTMachineData.Rows(number_machine).Item(0).ToString
        Dim no_product_in_hour As Double

        query_string = "Select * FROM hourly_output_data where id = '" & hourly_record_id & "'"
        select_product.Connection = connnection
        select_product.CommandText = query_string
        myAdapter.SelectCommand = select_product
        myAdapter.Fill(data_trolley)

        Dim product_record As String = data_trolley.Rows(0).Item("product_id").ToString
        Dim start_date_time As DateTime = data_trolley.Rows(0).Item("datetime")


        If product_record = "" Then
            query_update_record = "UPDATE hourly_output_data  SET product_id = '" & product & "', ct_target = '" & ct_target &
                            "' where id = '" & hourly_record_id & "'"
            update_record.Connection = connnection
            update_record.CommandText = query_update_record
            update_record.ExecuteNonQuery()
        ElseIf product_record <> product Then
            'update last hour
            update_time_and_duration_last_hourly_output_id(hourly_record_id)
            'create new product
            no_product_in_hour = get_no_product_in_hour(shift_hour_id) + 1
            create_hourly_output_records(machine_id, product, shift_id, shift_hour_id, ct_target, number_machine)

            DTMachineData.Rows(number_machine).Item(11) = no_product_in_hour
            DTMachineData.Rows(number_machine).Item(12) = shift_hour_id & String.Format("{0:000#}", no_product_in_hour)
            DTMachineData.Rows(number_machine).Item(13) = DateTime.Now.ToString("HH:mm:ss")
            DTMachineData.Rows(number_machine).Item(14) = ""
        End If
    End Sub

#End Region

#Region "monitoring Output"

    Private Sub update_temp_output(ByVal machine_id As String, ByVal qty As Double)
        Dim find_machine_id As Boolean = False
        Dim machine_number As Integer
        For i As Integer = 0 To DTMachineData.Rows.Count - 1
            If machine_id = DTMachineData.Rows(i).Item(0) Then
                find_machine_id = True
                machine_number = i
            End If
        Next

        If find_machine_id Then
            Dim start_date_time As DateTime = DTMachineData.Rows(machine_number).Item(27).ToString
            Dim end_time As DateTime = Now
            Dim duration As TimeSpan = end_time.Subtract(start_date_time)
            Dim ct_current As Double = duration.TotalSeconds / qty
            Dim actual_output As Double = DTMachineData.Rows(machine_number).Item(29) + qty
            Dim ct_average As Double = (DTMachineData.Rows(machine_number).Item(29) * DTMachineData.Rows(machine_number).Item(30) +
                                        qty * ct_current) / (DTMachineData.Rows(machine_number).Item(29) + qty)
            Dim ct_minimum As Double
            Dim ct_maximum As Double
            Dim downtime As Double = DTMachineData.Rows(machine_number).Item(33)
            Dim speed_loss As Double = DTMachineData.Rows(machine_number).Item(34)
            Dim number_speed_loss As Double = DTMachineData.Rows(machine_number).Item(35)
            Dim minor_stoppage As Double = DTMachineData.Rows(machine_number).Item(36)
            Dim number_minor_stoppage As Double = DTMachineData.Rows(machine_number).Item(37)

            If DTMachineData.Rows(machine_number).Item(31) = 0 Then
                ct_minimum = ct_current
            ElseIf ct_current > DTMachineData.Rows(machine_number).Item(31) Then
                ct_minimum = DTMachineData.Rows(machine_number).Item(31)
            Else
                ct_minimum = ct_current
            End If

            If ct_current < DTMachineData.Rows(machine_number).Item(32) Then
                ct_maximum = DTMachineData.Rows(machine_number).Item(32)
            Else
                ct_maximum = ct_current
            End If

            If DTMachineData.Rows(machine_number).Item(38) = 0 Then 'no product
                If ct_current > 300 Then
                    downtime += ct_current
                ElseIf ct_current > 120 Then
                    minor_stoppage += ct_current
                    number_minor_stoppage = +1
                ElseIf ct_current > 60 Then
                    speed_loss += ct_current
                    number_speed_loss += 1
                End If
            Else

            End If


            DTMachineData.Rows(machine_number).Item(27) = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
            DTMachineData.Rows(machine_number).Item(28) = ct_current.ToString("N2")
            DTMachineData.Rows(machine_number).Item(29) = actual_output
            DTMachineData.Rows(machine_number).Item(30) = ct_average.ToString("N2")
            DTMachineData.Rows(machine_number).Item(31) = ct_minimum.ToString("N2")
            DTMachineData.Rows(machine_number).Item(32) = ct_maximum.ToString("N2")
            DTMachineData.Rows(machine_number).Item(33) = downtime.ToString("N2")
            DTMachineData.Rows(machine_number).Item(34) = speed_loss.ToString("N2")
            DTMachineData.Rows(machine_number).Item(35) = number_speed_loss.ToString("N0")
            DTMachineData.Rows(machine_number).Item(36) = minor_stoppage.ToString("N2")
            DTMachineData.Rows(machine_number).Item(37) = number_minor_stoppage.ToString("N0")

            view_machine_data_hourly(machine_number)
            changeColorSelectedMachine(machine_number + 1)
        End If
    End Sub
#End Region

    Private Sub Form1_close(sender As Object, e As EventArgs) Handles MyBase.Closing
        DisconnectOPCServer()
    End Sub

#Region "auto closing"
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            'NotifyIcon1.Icon = SystemIcons.Application
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "oee data acquisition"
            NotifyIcon1.BalloonTipText = "oee data acquisition"
            NotifyIcon1.ShowBalloonTip(10)
            'Me.Hide()
            ShowInTaskbar = False
            auto_closing = True
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        'Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub


    Private Const CP_NOCLOSE_BUTTON As Integer = &H200
    Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim myCp As CreateParams = MyBase.CreateParams
            myCp.ClassStyle = myCp.ClassStyle Or CP_NOCLOSE_BUTTON
            Return myCp
        End Get
    End Property

    Private Sub lbl_title_Click(sender As Object, e As EventArgs) Handles lbl_title.Click
        auto_closing = False
    End Sub
#End Region

End Class

