using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;
namespace 調劑台管理系統
{
    public enum enum_儲位資訊
    {
        IP,
        TYPE,
        效期,
        庫存,
        異動量,
        Value,
    }
    public partial class Form1 : Form
    {
        public void Function_從SQL取得儲位到入賬資料(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = this.List_EPD583_雲端資料.SortByCode(藥品碼);
            List<Storage> storages = this.List_EPD266_雲端資料.SortByCode(藥品碼);
            List<Storage> pannels = this.List_Pannel35_雲端資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_雲端資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_雲端資料.SortByCode(藥品碼);
            for (int i = 0; i < boxes.Count; i++)
            {
                Drawer drawer = this.drawerUI_EPD_583.SQL_GetDrawer(boxes[i]);              
                this.List_EPD583_入賬資料.Add_NewDrawer(drawer);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                Storage storage = this.storageUI_EPD_266.SQL_GetStorage(storages[i]);
                this.List_EPD266_入賬資料.Add_NewStorage(storage);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                Storage pannel = this.storageUI_WT32.SQL_GetStorage(pannels[i]);
                this.List_Pannel35_入賬資料.Add_NewStorage(pannel);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                RowsLED rowsLED = this.rowsLEDUI.SQL_GetRowsLED(rowsDevices[i].IP);
                this.List_RowsLED_入賬資料.Add_NewRowsLED(rowsLED);
            }
            for (int i = 0; i < rFIDDevices.Count; i++)
            {
                RFIDClass rFIDClass = this.rfiD_UI.SQL_GetRFIDClass(rFIDDevices[i].IP);
                this.List_RFID_入賬資料.Add_NewRFIDClass(rFIDClass);
            }
        }
        public int Function_從入賬資料取得庫存(string 藥品碼)
        {
            int 庫存 = 0;
            List<object> list_value = new List<object>();
            List<string> 儲位_TYPE = new List<string>();
            this.Function_從入賬資料取得儲位(this.Function_藥品碼檢查(藥品碼), ref 儲位_TYPE, ref list_value);

            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i] is Device)
                {
                    庫存 += ((Device)list_value[i]).Inventory.StringToInt32();
                }
            }
            if (list_value.Count == 0) return -999;
            return 庫存;
        }
        public void Function_從入賬資料取得儲位(string 藥品碼, ref List<string> TYPE, ref List<object> values)
        {
            List<object> list_value = this.Function_從入賬資料取得儲位(藥品碼);
            TYPE.Clear();
            values.Clear();
            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i] is Device)
                {
                    Device device = (Device)list_value[i];
                    values.Add(list_value[i]);
                    TYPE.Add(device.DeviceType.GetEnumName());
                }

            }
        }
        public List<object> Function_從入賬資料取得儲位(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = this.List_EPD583_入賬資料.SortByCode(藥品碼);
            List<Storage> storages = this.List_EPD266_入賬資料.SortByCode(藥品碼);
            List<Storage> pannels = this.List_Pannel35_入賬資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_入賬資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_入賬資料.SortByCode(藥品碼);
            for (int i = 0; i < boxes.Count; i++)
            {
                list_value.Add(boxes[i]);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                list_value.Add(storages[i]);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                list_value.Add(pannels[i]);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                list_value.Add(rowsDevices[i]);
            }
            for (int i = 0; i < rFIDDevices.Count; i++)
            {
                list_value.Add(rFIDDevices[i]);
            }
            return list_value;
        }

        public void Function_設定雲端資料更新()
        {
            this.Function_取藥堆疊資料_新增母資料(Guid.NewGuid().ToString(), "更新資料", enum_交易記錄查詢動作.None, "", "", "", "", "", "", "", "", "", "", 0, "");
        }
        public void Function_從SQL取得儲位到雲端資料()
        {
            try
            {
                MyTimer myTimer = new MyTimer();
                myTimer.StartTickTime(50000);
                Console.WriteLine($"開始SQL讀取儲位資料到雲端!");
                List<Task> taskList = new List<Task>();
                taskList.Add(Task.Run(() =>
                {
                    MyTimer myTimer0 = new MyTimer();
                    myTimer0.StartTickTime(50000);
                    List_EPD583_雲端資料 = this.drawerUI_EPD_583.SQL_GetAllDrawers();
                    Console.WriteLine($"讀取EPD583資料! 耗時 :{myTimer0.GetTickTime().ToString("0.000")} ");
                }));
                taskList.Add(Task.Run(() =>
                {
                    MyTimer myTimer1 = new MyTimer();
                    myTimer1.StartTickTime(50000);
                    List_EPD266_雲端資料 = this.storageUI_EPD_266.SQL_GetAllStorage();
                    Console.WriteLine($"讀取EPD266資料! 耗時 :{myTimer1.GetTickTime().ToString("0.000")} ");

                }));
                taskList.Add(Task.Run(() =>
                {
                    MyTimer myTimer2 = new MyTimer();
                    myTimer2.StartTickTime(50000);
                    List_RowsLED_雲端資料 = this.rowsLEDUI.SQL_GetAllRowsLED();
                    Console.WriteLine($"讀取RowsLED資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

                }));
                taskList.Add(Task.Run(() =>
                {
                    MyTimer myTimer2 = new MyTimer();
                    myTimer2.StartTickTime(50000);
                    List_Pannel35_雲端資料 = this.storageUI_WT32.SQL_GetAllStorage();
                    Console.WriteLine($"讀取Pannel35資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

                }));
                taskList.Add(Task.Run(() =>
                {
                    MyTimer myTimer2 = new MyTimer();
                    myTimer2.StartTickTime(50000);
                    List_RFID_雲端資料 = this.rfiD_UI.SQL_GetAllRFIDClass();
                    Console.WriteLine($"外部設備資料資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

                }));
                Task allTask = Task.WhenAll(taskList);
                allTask.Wait();
                Console.WriteLine($"SQL讀取儲位資料到雲端結束! 耗時 : {myTimer.GetTickTime().ToString("0.000")}");
            }
            catch
            {

            }
        
        }
        public List<object> Function_從SQL取得儲位到雲端資料(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = this.List_EPD583_雲端資料.SortByCode(藥品碼);
            List<Storage> storages = this.List_EPD266_雲端資料.SortByCode(藥品碼);
            List<Storage> pannels = this.List_Pannel35_雲端資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_雲端資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_雲端資料.SortByCode(藥品碼);
            for (int i = 0; i < boxes.Count; i++)
            {
                Box box = this.drawerUI_EPD_583.SQL_GetBox(boxes[i]);
                this.List_EPD583_雲端資料.Add_NewDrawer(box);
                list_value.Add(box);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                Storage storage = this.storageUI_EPD_266.SQL_GetStorage(storages[i]);
                this.List_EPD266_雲端資料.Add_NewStorage(storage);
                list_value.Add(storage);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                Storage pannel = this.storageUI_WT32.SQL_GetStorage(pannels[i]);
                this.List_Pannel35_雲端資料.Add_NewStorage(pannel);
                list_value.Add(pannel);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                RowsLED rowsLED = this.rowsLEDUI.SQL_GetRowsLED(rowsDevices[i].IP);
                RowsDevice rowsDevice = rowsLED.GetRowsDevice(rowsDevices[i].GUID);
                if (rowsDevice != null) list_value.Add(rowsDevice);
                this.List_RowsLED_雲端資料.Add_NewRowsLED(rowsDevice);
            }
            for (int i = 0; i < rFIDDevices.Count; i++)
            {
                RFIDDevice rFIDDevice = this.rfiD_UI.SQL_GetDevice(rFIDDevices[i]);
                this.List_RFID_雲端資料.Add_NewRFIDClass(rFIDDevice);
                list_value.Add(rFIDDevices);
            }
            return list_value;
        }    
        public List<object> Function_從雲端資料取得儲位(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = this.List_EPD583_雲端資料.SortByCode(藥品碼);
            List<Storage> storages = this.List_EPD266_雲端資料.SortByCode(藥品碼);
            List<Storage> pannels = this.List_Pannel35_雲端資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_雲端資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_雲端資料.SortByCode(藥品碼);
            for (int i = 0; i < boxes.Count; i++)
            {
                list_value.Add(boxes[i]);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                list_value.Add(storages[i]);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                list_value.Add(pannels[i]);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                list_value.Add(rowsDevices[i]);
            }
            for(int i = 0; i < rFIDDevices.Count; i++)
            {
                list_value.Add(rFIDDevices[i]);
            }
            return list_value;
        }
        public void Function_從雲端資料取得儲位(string 藥品碼, ref List<string> TYPE, ref List<object> values)
        {
            List<object> list_value = this.Function_從雲端資料取得儲位(藥品碼);
            TYPE.Clear();
            values.Clear();
            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i] is Device)
                {
                    Device device = (Device)list_value[i];
                    values.Add(list_value[i]);
                    TYPE.Add(device.DeviceType.GetEnumName());
                }

            }
        }
        public int Function_從雲端資料取得庫存(string 藥品碼)
        {
            int 庫存 = 0;
            List<object> list_value = new List<object>();
            List<string> 儲位_TYPE = new List<string>();
            this.Function_從雲端資料取得儲位(this.Function_藥品碼檢查(藥品碼), ref 儲位_TYPE, ref list_value);

            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i] is Device)
                {
                    庫存 += ((Device)list_value[i]).Inventory.StringToInt32();
                }
            }
            if (list_value.Count == 0) return -999;
            return 庫存;
        }
        public List<object[]> Function_取得異動儲位資訊從雲端資料(string 藥品碼, int 異動量, string 效期 ,string IP)
        {
            List<object> 儲位 = new List<object>();
            List<string> 儲位_TYPE = new List<string>();
            this.Function_從雲端資料取得儲位(this.Function_藥品碼檢查(藥品碼), ref 儲位_TYPE, ref 儲位);
            List<object[]> 儲位資訊_buf = new List<object[]>();
            List<object[]> 儲位資訊 = new List<object[]>();
            for (int k = 0; k < 儲位.Count; k++)
            {
                object value_device = 儲位[k];
                object[] value = new object[new enum_儲位資訊().GetLength()];
                if (value_device is Device)
                {
                    Device device = (Device)value_device;
                    for (int i = 0; i < device.List_Validity_period.Count; i++)
                    {
                        if (device.List_Validity_period[i] == 效期 && device.IP == IP)
                        {
                            value[(int)enum_儲位資訊.IP] = device.IP;
                            value[(int)enum_儲位資訊.TYPE] = 儲位_TYPE[k];
                            value[(int)enum_儲位資訊.效期] = device.List_Validity_period[i];
                            value[(int)enum_儲位資訊.庫存] = device.List_Inventory[i];
                            value[(int)enum_儲位資訊.異動量] = 異動量.ToString();
                            value[(int)enum_儲位資訊.Value] = value_device;
                            儲位資訊.Add(value);
                            break;
                        }
                    }
                }
            }
            return 儲位資訊;
        }
        public List<object[]> Function_取得異動儲位資訊從雲端資料(string 藥品碼, int 異動量, string 效期)
        {
            List<object> 儲位 = new List<object>();
            List<string> 儲位_TYPE = new List<string>();
            this.Function_從雲端資料取得儲位(this.Function_藥品碼檢查(藥品碼), ref 儲位_TYPE, ref 儲位);
            List<object[]> 儲位資訊_buf = new List<object[]>();
            List<object[]> 儲位資訊 = new List<object[]>();
            for (int k = 0; k < 儲位.Count; k++)
            {
                object value_device = 儲位[k];
                object[] value = new object[new enum_儲位資訊().GetLength()];
                if (value_device is Device)
                {
                    Device device = (Device)value_device;
                    for (int i = 0; i < device.List_Validity_period.Count; i++)
                    {
                        if (device.List_Validity_period[i] == 效期)
                        {
                            value[(int)enum_儲位資訊.IP] = device.IP;
                            value[(int)enum_儲位資訊.TYPE] = 儲位_TYPE[k];
                            value[(int)enum_儲位資訊.效期] = device.List_Validity_period[i];
                            value[(int)enum_儲位資訊.庫存] = device.List_Inventory[i];
                            value[(int)enum_儲位資訊.異動量] = 異動量.ToString();
                            value[(int)enum_儲位資訊.Value] = value_device;
                            儲位資訊.Add(value);
                            break;
                        }
                    }
                }
            }
            return 儲位資訊;
        }
        public List<object[]> Function_取得異動儲位資訊從雲端資料(string 藥品碼, int 異動量)
        {
            List<object> 儲位 = new List<object>();
            List<string> 儲位_TYPE = new List<string>();
            this.Function_從雲端資料取得儲位(this.Function_藥品碼檢查(藥品碼), ref 儲位_TYPE, ref 儲位);
            List<object[]> 儲位資訊_buf = new List<object[]>();
            List<object[]> 儲位資訊 = new List<object[]>();
            if (儲位.Count == 0) return 儲位資訊_buf;


            for (int k = 0; k < 儲位.Count; k++)
            {
                object value_device = 儲位[k];              
                if (value_device is Device)
                {
                    Device device = (Device)value_device;
                    for (int i = 0; i < device.List_Validity_period.Count; i++)
                    {
                        object[] value = new object[new enum_儲位資訊().GetLength()];
                        value[(int)enum_儲位資訊.IP] = device.IP;
                        value[(int)enum_儲位資訊.TYPE] = 儲位_TYPE[k];
                        value[(int)enum_儲位資訊.效期] = device.List_Validity_period[i];
                        value[(int)enum_儲位資訊.庫存] = device.List_Inventory[i];
                        value[(int)enum_儲位資訊.異動量] = "0";
                        value[(int)enum_儲位資訊.Value] = value_device;
                        儲位資訊.Add(value);
                    }
                }
            }
            儲位資訊 = 儲位資訊.OrderBy(r => DateTime.Parse(r[(int)enum_儲位資訊.效期].ToDateString())).ToList();

            if (異動量 == 0) return 儲位資訊;
            int 使用數量 = 異動量;
            int 庫存數量 = 0;
            int 剩餘庫存數量 = 0;
            for (int i = 0; i < 儲位資訊.Count; i++)
            {
                庫存數量 = 儲位資訊[i][(int)enum_儲位資訊.庫存].ObjectToString().StringToInt32();
                if ((使用數量 < 0 && 庫存數量 > 0) || (使用數量 > 0 && 庫存數量 >= 0))
                {
                    剩餘庫存數量 = 庫存數量 + 使用數量;
                    if (剩餘庫存數量 >= 0)
                    {
                        儲位資訊[i][(int)enum_儲位資訊.異動量] = (使用數量).ToString();
                        儲位資訊_buf.Add(儲位資訊[i]);
                        break;
                    }
                    else
                    {
                        儲位資訊[i][(int)enum_儲位資訊.異動量] = (庫存數量 * -1).ToString();
                        使用數量 = 剩餘庫存數量;
                        儲位資訊_buf.Add(儲位資訊[i]);
                    }
                }
            }

            return 儲位資訊_buf;
        }
        public object Function_庫存異動至雲端資料(object[] 儲位資訊)
        {
            return this.Function_庫存異動至雲端資料(儲位資訊, false);
        }
        public object Function_庫存異動至雲端資料(object[] 儲位資訊 ,bool upToSQL)
        {
            object Value = 儲位資訊[(int)enum_儲位資訊.Value];
            string 效期 = 儲位資訊[(int)enum_儲位資訊.效期].ObjectToString();
            string 異動量 = 儲位資訊[(int)enum_儲位資訊.異動量].ObjectToString();
            string TYPE = 儲位資訊[(int)enum_儲位資訊.TYPE].ObjectToString();
            if (Value is Storage)
            {
                if (TYPE == DeviceType.EPD266.GetEnumName() || TYPE == DeviceType.EPD266_lock.GetEnumName())
                {
                    Storage storage = (Storage)Value;
                    storage = this.List_EPD266_雲端資料.SortByIP(storage.IP);
                    if (storage != null)
                    {
                        storage.效期庫存異動(效期, 異動量, false);
                        this.List_EPD266_雲端資料.Add_NewStorage(storage);
                        if (upToSQL) this.storageUI_EPD_266.SQL_ReplaceStorage(storage);
                        storage.UpToSQL = true;
                        return storage;
                    }
                }
                if (TYPE == DeviceType.Pannel35.GetEnumName() || TYPE == DeviceType.Pannel35_lock.GetEnumName())
                {
                    Storage storage = (Storage)Value;
                    storage = this.List_Pannel35_雲端資料.SortByIP(storage.IP);
                    if (storage != null)
                    {
                        storage.效期庫存異動(效期, 異動量, false);
                        this.List_Pannel35_雲端資料.Add_NewStorage(storage);
                        if (upToSQL) this.storageUI_WT32.SQL_ReplaceStorage(storage);
                        storage.UpToSQL = true;
                        return storage;
                    }
                }
            }
            else if (Value is Box)
            {
                if (TYPE == DeviceType.EPD583.GetEnumName() || TYPE == DeviceType.EPD583_lock.GetEnumName())
                {
                    Box box = (Box)Value;
                    box.效期庫存異動(效期, 異動量, false);
                    this.List_EPD583_雲端資料.ReplaceBox(box);
                    Drawer drawer = this.List_EPD583_雲端資料.SortByIP(box.IP);
                    if (upToSQL) this.drawerUI_EPD_583.SQL_ReplaceDrawer(drawer);
                    drawer.UpToSQL = true;
                    return drawer;
                }        
            }
            else if (Value is RowsDevice)
            {
                if (TYPE == DeviceType.RowsLED.GetEnumName())
                {
                    RowsDevice rowsDevice = Value as RowsDevice;
                    rowsDevice.效期庫存異動(效期, 異動量, false);
                    this.List_RowsLED_雲端資料.Add_NewRowsLED(rowsDevice);
                    RowsLED rowsLED = this.List_RowsLED_雲端資料.SortByIP(rowsDevice.IP);
                    if (upToSQL) this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                    rowsLED.UpToSQL = true;
                    return rowsLED;
                }
                
            }
            else if(Value is RFIDDevice)
            {
                if (TYPE == DeviceType.RFID_Device.GetEnumName())
                {
                    RFIDDevice rFIDDevice = Value as RFIDDevice;
                    rFIDDevice.效期庫存異動(效期, 異動量, false);
                    this.List_RFID_雲端資料.Add_NewRFIDClass(rFIDDevice);
                    RFIDClass rFIDClass = this.List_RFID_雲端資料.SortByIP(rFIDDevice.IP);
                    if (upToSQL) this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
                    rFIDClass.UpToSQL = true;
                    return rFIDClass;
                }           
            }
            return null;
        }
        public void Function_雲端資料上傳至SQL()
        {
            List<Storage> list_EPD266 = this.List_EPD266_雲端資料.GetUpToSQL();
            List<Storage> list_Pannel35 = this.List_Pannel35_雲端資料.GetUpToSQL();
            List<Drawer> list_EPD583 = this.List_EPD583_雲端資料.GetUpToSQL();
            List<RowsLED> list_RowsLED = this.List_RowsLED_雲端資料.GetUpToSQL();
            List<RFIDClass> list_RFID = this.List_RFID_雲端資料.GetUpToSQL();

            if (list_EPD266.Count > 0) this.storageUI_EPD_266.SQL_ReplaceStorage(list_EPD266);
            if (list_Pannel35.Count > 0) this.storageUI_WT32.SQL_ReplaceStorage(list_Pannel35);
            if (list_EPD583.Count > 0) this.drawerUI_EPD_583.SQL_ReplaceDrawer(list_EPD583);
            if (list_RowsLED.Count > 0) this.rowsLEDUI.SQL_ReplaceRowsLED(list_RowsLED);
            if (list_RFID.Count > 0) this.rfiD_UI.SQL_ReplaceRFIDClass(list_RFID);

        }


  

        public void Function_從SQL取得儲位到本地資料()
        {

            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            Console.WriteLine($"開始SQL讀取儲位資料到本地!");
            List<Task> taskList = new List<Task>();
            taskList.Add(Task.Run(() =>
            {
                MyTimer myTimer0 = new MyTimer();
                myTimer0.StartTickTime(50000);
                List_EPD583_本地資料 = this.drawerUI_EPD_583.SQL_GetAllDrawers();
                Console.WriteLine($"讀取EPD583資料! 耗時 :{myTimer0.GetTickTime().ToString("0.000")} ");
            }));
            taskList.Add(Task.Run(() =>
            {
                MyTimer myTimer1 = new MyTimer();
                myTimer1.StartTickTime(50000);
                List_EPD266_本地資料 = this.storageUI_EPD_266.SQL_GetAllStorage();
                Console.WriteLine($"讀取EPD266資料! 耗時 :{myTimer1.GetTickTime().ToString("0.000")} ");

            }));
            taskList.Add(Task.Run(() =>
            {
                MyTimer myTimer2 = new MyTimer();
                myTimer2.StartTickTime(50000);
                List_RowsLED_本地資料 = this.rowsLEDUI.SQL_GetAllRowsLED();
                Console.WriteLine($"讀取RowsLED資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

            }));
            taskList.Add(Task.Run(() =>
            {
                MyTimer myTimer2 = new MyTimer();
                myTimer2.StartTickTime(50000);
                List_RFID_本地資料 = this.rfiD_UI.SQL_GetAllRFIDClass();
                Console.WriteLine($"外部設備資料資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

            }));
            taskList.Add(Task.Run(() =>
            {
                MyTimer myTimer2 = new MyTimer();
                myTimer2.StartTickTime(50000);
                List_Pannel35_本地資料 = this.storageUI_WT32.SQL_GetAllStorage();
                Console.WriteLine($"讀取Pannel35資料! 耗時 :{myTimer2.GetTickTime().ToString("0.000")} ");

            }));
            Task allTask = Task.WhenAll(taskList);
            allTask.Wait();
            Console.WriteLine($"SQL讀取儲位資料到本地結束! 耗時 : {myTimer.GetTickTime().ToString("0.000")}");
        }
        public List<object> Function_從本地資料取得儲位(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = List_EPD583_本地資料.SortByCode(藥品碼);
            List<Storage> storages = List_EPD266_本地資料.SortByCode(藥品碼);
            List<Storage> pannels = List_Pannel35_本地資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_本地資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_本地資料.SortByCode(藥品碼);
            for (int i = 0; i < boxes.Count; i++)
            {
                list_value.Add(boxes[i]);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                list_value.Add(storages[i]);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                list_value.Add(pannels[i]);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                list_value.Add(rowsDevices[i]);
            }
            for (int i = 0; i < rFIDDevices.Count; i++)
            {
                list_value.Add(rFIDDevices[i]);
            }
            return list_value;
        }
        public object Fucnction_從本地資料取得儲位(string IP)
        {
            Storage storage = this.List_EPD266_本地資料.SortByIP(IP);
            if (storage != null) return storage;
            Drawer drawer = this.List_EPD583_本地資料.SortByIP(IP);
            if (drawer != null) return drawer;
            RowsLED rowsLED = this.List_RowsLED_本地資料.SortByIP(IP);
            if (rowsLED != null) return rowsLED;
            RFIDClass rFIDClass = this.List_RFID_本地資料.SortByIP(IP);
            if (rFIDClass != null) return rFIDClass;
            Storage pannel35 = this.List_Pannel35_本地資料.SortByIP(IP);
            if (pannel35 != null) return pannel35;
            return null;
        }
        public List<Device> Function_從SQL取得所有儲位()
        {
            List<List<Device>> list_list_devices = new List<List<Device>>();
            List<Device> devices = new List<Device>();
            this.Function_從SQL取得儲位到本地資料();
            List<Box> boxes = this.List_EPD583_本地資料.GetAllBoxes();
            list_list_devices.Add(this.List_EPD583_本地資料.GetAllDevice());
            list_list_devices.Add(this.List_EPD266_本地資料.GetAllDevice());
            list_list_devices.Add(this.List_RowsLED_本地資料.GetAllDevice());
            list_list_devices.Add(this.List_RFID_本地資料.GetAllDevice());
            list_list_devices.Add(this.List_Pannel35_本地資料.GetAllDevice());

            for (int i = 0; i < list_list_devices.Count; i++)
            {
                foreach(Device device in list_list_devices[i])
                {
                    device.確認效期庫存(true);
                    devices.Add(device);
                }
            }
            return devices;
        }
        public List<object> Function_從SQL取得儲位到本地資料(string 藥品碼)
        {
            List<object> list_value = new List<object>();
            List<Box> boxes = this.List_EPD583_本地資料.SortByCode(藥品碼);
            List<Storage> storages = this.List_EPD266_本地資料.SortByCode(藥品碼);
            List<RowsDevice> rowsDevices = this.List_RowsLED_本地資料.SortByCode(藥品碼);
            List<RFIDDevice> rFIDDevices = this.List_RFID_本地資料.SortByCode(藥品碼);
            List<Storage> pannels = this.List_Pannel35_本地資料.SortByCode(藥品碼);

            for (int i = 0; i < boxes.Count; i++)
            {
                Box box = this.drawerUI_EPD_583.SQL_GetBox(boxes[i]);
                this.List_EPD583_本地資料.Add_NewDrawer(box);
                list_value.Add(box);
            }
            for (int i = 0; i < storages.Count; i++)
            {
                Storage storage = this.storageUI_EPD_266.SQL_GetStorage(storages[i]);
                this.List_EPD266_本地資料.Add_NewStorage(storage);
                list_value.Add(storage);
            }
            for (int i = 0; i < pannels.Count; i++)
            {
                Storage pannel = this.storageUI_WT32.SQL_GetStorage(pannels[i]);
                this.List_Pannel35_本地資料.Add_NewStorage(pannel);
                list_value.Add(pannel);
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                RowsDevice rowsDevice = this.rowsLEDUI.SQL_GetRowsDevice(rowsDevices[i]);
                this.List_RowsLED_本地資料.Add_NewRowsLED(rowsDevice);
                list_value.Add(rowsDevice);
            }
            for (int i = 0; i < rFIDDevices.Count; i++)
            {
                RFIDDevice rFIDDevice = this.rfiD_UI.SQL_GetDevice(rFIDDevices[i]);
                list_value.Add(rFIDDevice);
            }
            return list_value;
        }
        public int Function_從SQL取得庫存(string 藥品碼)
        {
            int 庫存 = 0;
            List<object> list_value = this.Function_從SQL取得儲位到本地資料(藥品碼);
            for (int i = 0; i < list_value.Count; i++)
            {

                if (list_value[i] is Device)
                {
                    Device device = list_value[i] as Device;
                    if (device != null)
                    {
                        庫存 += device.Inventory.StringToInt32();
                    }
                }
             
            }
            return 庫存;
        }
        public int Function_從本地資料取得庫存(string 藥品碼)
        {
            int 庫存 = 0;
            List<object> list_value = this.Function_從本地資料取得儲位(藥品碼);
            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i] is Device)
                {
                    Device device = (Device)list_value[i];
                    if (device != null)
                    {
                        庫存 += device.Inventory.StringToInt32();
                    }
                }
            }
            return 庫存;
        }

        public void Function_儲位亮燈(string 藥品碼, Color color)
        {
            List<string> list_lock_IP = new List<string>();
            this.Function_儲位亮燈(藥品碼, color ,ref list_lock_IP);
        }
        public void Function_儲位亮燈(string 藥品碼 ,Color color, ref List<string> list_lock_IP)
        {
            List<object> list_Device = this.Function_從本地資料取得儲位(藥品碼);
            List<Task> taskList = new List<Task>();
            List<string> list_IP = new List<string>();
            List<string> list_IP_buf = new List<string>();
            for (int i = 0; i < list_Device.Count; i++)
            {
                Device device = list_Device[i] as Device;
                string IP = device.IP;

                list_IP_buf = (from value in list_IP
                               where value == IP
                               select value).ToList();
                if (list_IP_buf.Count > 0) continue;

                if (device != null)
                {
                    if (device.DeviceType == DeviceType.EPD266 || device.DeviceType == DeviceType.EPD266_lock)
                    {
                        Storage storage = list_Device[i] as Storage;
                        if (storage != null)
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, color);
                            }));
                            list_IP.Add(IP);
                            if (device.DeviceType == DeviceType.EPD266_lock) list_lock_IP.Add(IP);
                        }
                    }
                    else if (device.DeviceType == DeviceType.EPD583 || device.DeviceType == DeviceType.EPD583_lock)
                    {
                        Box box = list_Device[i] as Box;
                        if (box != null)
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                Drawer drawer = List_EPD583_雲端資料.SortByIP(IP);
                                List<Box> boxes = drawer.SortByCode(藥品碼);

                                if (drawer.IsAllLight)
                                {
                                    drawer.LED_Bytes = DrawerUI_EPD_583.Set_LEDBytes(drawer, boxes, color);
                                    drawer.LED_Bytes = DrawerUI_EPD_583.Set_Pannel_LEDBytes(drawer, color);
                                }
                                else
                                {
                                    drawer.LED_Bytes = DrawerUI_EPD_583.Set_LEDBytes(drawer, color);
                                }

                                this.drawerUI_EPD_583.Set_LED_UDP(drawer);
                            }));
                            list_IP.Add(IP);
                            if (device.DeviceType == DeviceType.EPD583_lock) list_lock_IP.Add(IP);
                        }
                    }
                    else if (device.DeviceType == DeviceType.Pannel35)
                    {
                        Storage storage = list_Device[i] as Storage;
                        if (storage != null)
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                this.storageUI_WT32.Set_Stroage_LED_UDP(storage, color);
                            }));
                            list_IP.Add(IP);
                        }
                    }
                    else if (device.DeviceType == DeviceType.RowsLED)
                    {
                        RowsDevice rowsDevice = list_Device[i] as RowsDevice;
                        if (rowsDevice != null)
                        {
                            RowsLED rowsLED = List_RowsLED_雲端資料.SortByIP(rowsDevice.IP);
                            rowsLED.LED_Bytes = RowsLEDUI.Get_Rows_LEDBytes(ref rowsLED.LED_Bytes, rowsDevice, color);                       
                            taskList.Add(Task.Run(() =>
                            {
                                this.rowsLEDUI.Set_Rows_LED_UDP(rowsLED);
                            }));
                            list_IP.Add(IP);
                        }
                    }
                }
            }




            //Task allTask = Task.WhenAll(taskList);
            //allTask.Wait();
        }

        public string Function_取得藥品網址(string 藥品碼)
        {
            if (藥品碼.Length < 5) 藥品碼 = "0" + 藥品碼;
            string URL = $@"{myConfigClass.藥物辨識網址}{藥品碼}.jpg";
            return string.Format(URL, 藥品碼);
        }
        public void Function_顯示藥物辨識圖片(string 藥品碼, PictureBox pictureBox)
        {
            if (myConfigClass.藥物辨識網址.StringIsEmpty() == false)
            {
                this.Invoke(new Action(delegate
                {
                    string URL = this.Function_取得藥品網址(藥品碼);
                    Basic.Net.DowloadToPictureBox(URL, pictureBox);
                }));

            }
        }
        public string Function_藥品碼檢查(string Code)
        {
     
            return Code;
        }


    }


}
