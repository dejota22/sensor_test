using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class DataGlobalModel
    {
        public string id;
        public int alarm;
        public DateTime dataReceive;
        public string tipo;

        public int motorId;
        public string motor;
        public int deviceId;
        public string device;
    }

    public class DataGlobalHomeModel
    {
        public DateTime dataReceive;

        public int? companyId;

        public int? unitId;
        public string unitName;
        public int? sectorId;
        public string sectorName;
        public int? subSectorId;
        public string subSectorName;

        public int deviceId;
        public string device;
        public int motorId;
        public string motor;

        public string plan;
        public string orientation;

        public string alertaAccX;
        public string alertaAccY;
        public string alertaAccZ;

        public string alertaSpdX;
        public string alertaSpdY;
        public string alertaSpdZ;

        public string valorAccX;
        public string valorAccY;
        public string valorAccZ;

        public string valorSpdX;
        public string valorSpdY;
        public string valorSpdZ;

        public string alerta;
    }

    public class DataDeviceExport
    {
        public DateTime dataReceive;

        public int? companyId;
        public string companyName;

        public int? unitId;
        public string unitName;
        public int? sectorId;
        public string sectorName;
        public int? subSectorId;
        public string subSectorName;

        public int deviceId;
        public string device;
        public int motorId;
        public string motor;

        public string sensor;
        public string tipo;
        public string rms;
        public string fatorCrista;
        public string alarme;
        public string odr;
        public string freqCut;
        public string filtro;
        public string eixo;
        public string fs;
        public string amostras;
    }
}
