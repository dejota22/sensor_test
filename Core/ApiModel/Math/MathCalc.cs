//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Core.ApiModel.Math
//{
//        using tk = tkinter;

//        using font = tkinter.font;

//        using ttk = tkinter.ttk;

//        using np = numpy;

//        using fft = scipy.fftpack.fft;

//        using plt = matplotlib.pyplot;

//        using FigureCanvasTkAgg = matplotlib.backends.backend_tkagg.FigureCanvasTkAgg;

//        using serial;

//        using askyesno = tkinter.messagebox.askyesno;

//        using datetime = datetime.datetime;

//        using os;

//        using time;

//        using System;

//        using System.Collections.Generic;

//        using System.Linq;

//        public static class Module
//        {

//            static Module()
//            {
//                @"
//Console Terminal for Acceleration Sensor Interface
//";
//                root.title("Vibration Analysis Console");
//                root.geometry("1470x874");
//                root.resizable(false, false);
//                Button_Check["font"] = font.Font(size: 16, family: "Helvetica");
//                Button_Check.grid(row: 0, column: 0, columnspan: 2, padx: 2, pady: 2, sticky: "N");
//                Button_Mem["font"] = font.Font(size: 16, family: "Helvetica");
//                Button_Mem.grid(row: 1, column: 0, columnspan: 2, padx: 2, pady: 2, sticky: "N");
//                Button_Format["font"] = font.Font(size: 16, family: "Helvetica");
//                Button_Format.grid(row: 2, column: 0, columnspan: 2, padx: 2, pady: 2, sticky: "N");
//                Button_Acq["font"] = font.Font(size: 16, family: "Helvetica");
//                Button_Acq.grid(row: 3, column: 0, columnspan: 2, padx: 2, pady: 2, sticky: "N");
//                FREQ.grid(row: 10, column: 0, columnspan: 2, sticky: "W");
//                BDR.grid(row: 11, column: 0, columnspan: 2, sticky: "W");
//                CUTOFF.grid(row: 12, column: 0, columnspan: 2, sticky: "W");
//                LPHP.grid(row: 13, column: 0, columnspan: 2, sticky: "W");
//                LINES.grid(row: 14, column: 0, columnspan: 2, sticky: "W");
//                AXIS.grid(row: 15, column: 0, columnspan: 2, sticky: "W");
//                PXs.set(false);
//                PYs.set(true);
//                PZs.set(false);
//                Timer_cycle.grid(row: 21, columnspan: 3, sticky: "W");
//                Timer_cycle.insert("1.0", "1");
//                Button_Temp["font"] = font.Font(size: 16, family: "Helvetica");
//                Button_Temp.grid(row: 29, column: 0, columnspan: 2, padx: 2, pady: 2, sticky: "N");
//                plot1.plot(y, color: "b", linewidth: "2");
//                plot1.grid(true);
//                fig.tight_layout();
//                canvas.draw();
//                canvas.get_tk_widget().grid(row: 0, column: 2, padx: 4, pady: 4, rowspan: 10, columnspan: 3);
//                plot2.plot(y, color: "b", linewidth: "2", label: "data");
//                plot2.grid(true);
//                fig2.tight_layout();
//                canvas2.draw();
//                canvas2.get_tk_widget().grid(row: 10, column: 2, padx: 4, pady: 4, rowspan: 10, columnspan: 3);
//                plot3.plot(y, color: "b", linewidth: "2", label: "data");
//                plot3.grid(true);
//                fig3.tight_layout();
//                canvas3.draw();
//                canvas3.get_tk_widget().grid(row: 20, column: 2, padx: 4, pady: 4, rowspan: 10, columnspan: 3);
//                Status.grid(row: 30, columnspan: 3, sticky: "W");
//                scrollbar.grid(row: 30, column: 4, sticky: "ns");
//                Status["yscrollcommand"] = scrollbar.set;
//                root.update_idletasks();
//                root.update();
//                Sweep();
//            }

//            // -*- coding: utf-8 -*-
//            public static object Checar()
//            {
//                Status.delete("1.0", tk.END);
//                Status.insert("1.0", "Checking...\n");
//                Status.insert("2.0", comm("L$"));
//            }

//            public static object Sweep()
//            {
//                Status.delete("1.0", tk.END);
//                Status.insert("1.0", "X");
//                var AXIS_code = AXIS_TUPLE.index(AXIS._variable.get()).ToString();
//                if (AXIS_code == "0")
//                {
//                    Acq("X", true, true);
//                }
//                else if (AXIS_code == "1")
//                {
//                    Acq("Y", true, true);
//                }
//                else if (AXIS_code == "2")
//                {
//                    Acq("Z", true, true);
//                }
//                else if (AXIS_code == "3")
//                {
//                    Acq("X", true, PXs.get());
//                    Acq("Y", false, PYs.get());
//                    Acq("Z", false, PZs.get());
//                }
//            }

//            public static object save_acc_Array(object Ax, object acc_Array)
//            {
//                var header = "Sensor:,#1\n" + "FS:,16g\n" + "Motor freq.:," + FREQ._variable.get()[7 - FREQ._variable.get().Count] + "\n" + "BDR:," + BDR._variable.get()[5 - BDR._variable.get().Count] + "\n" + "Cutoff:," + CUTOFF._variable.get()[8 - CUTOFF._variable.get().Count] + "\n" + "Filter:," + LPHP._variable.get() + "\n" + "Lines:," + LINES._variable.get()[7 - LINES._variable.get().Count] + "\n" + "Axis:," + Ax + "\n\n" + "time [s],acc [g]";
//                var directory = FREQ._variable.get()[7 - FREQ._variable.get().Count];
//                try
//                {
//                    os.mkdir(directory);
//                }
//                catch
//                {
//                }
//                var dt_string = datetime.now().strftime("%Y-%m-%d-%H-%M-%S");
//                np.savetxt(directory + "/" + dt_string + "_" + Ax + ".csv", acc_Array, delimiter: ",", header: header, comments: "");
//                np.savetxt(directory + "/" + dt_string + "_" + Ax + ".txt", acc_Array, delimiter: ",", header: header, comments: "");
//            }

//            public static object Acq(object Ax, object clean, object plot)
//            {
//                object LINES_code;
//                object FDS_code;
//                object LPF2_code;
//                var msg = comm("P$");
//                var Sector = msg[-3];
//                Sector = Convert.ToInt32(Sector, 16);
//                Sector = hex(Sector)[2].zfill(3).upper();
//                Status.delete("1.0", tk.END);
//                var SENSOR_code = "1";
//                var FS_code = "1";
//                var BDR_code = BDR_TUPLE.index(BDR._variable.get()).ToString();
//                var LPHP_code = LPHP_TUPLE.index(LPHP._variable.get()).ToString();
//                if (LPHP_code == "1")
//                {
//                    LPF2_code = "1";
//                }
//                else
//                {
//                    LPF2_code = "0";
//                }
//                var CUTOFF_code = CUTOFF_TUPLE.index(CUTOFF._variable.get()).ToString();
//                if (LPHP_code == "0")
//                {
//                    FDS_code = "1";
//                }
//                else
//                {
//                    FDS_code = "0";
//                }
//                var AXIS_code = Ax;
//                if (BDR_code == "9")
//                {
//                    LINES_code = "3";
//                }
//                else
//                {
//                    LINES_code = LINES_TUPLE.index(LINES._variable.get()).ToString();
//                }
//                var ask = "S" + SENSOR_code + FS_code + BDR_code + LPF2_code + CUTOFF_code + FDS_code + AXIS_code + LINES_code + "$";
//                msg = comm(ask);
//                var Page = 1;
//                if (LINES_code == "0")
//                {
//                    Page = Convert.ToInt32(512 / 128);
//                }
//                if (LINES_code == "1")
//                {
//                    Page = Convert.ToInt32(1024 / 128);
//                }
//                if (LINES_code == "2")
//                {
//                    Page = Convert.ToInt32(2048 / 128);
//                }
//                if (LINES_code == "3")
//                {
//                    Page = Convert.ToInt32(3456 / 128);
//                }
//                if (LINES_code == "4")
//                {
//                    Page = Convert.ToInt32(4096 / 128);
//                }
//                if (LINES_code == "5")
//                {
//                    Page = Convert.ToInt32(6144 / 128);
//                }
//                if (LINES_code == "6")
//                {
//                    Page = Convert.ToInt32(8192 / 128);
//                }
//                if (LINES_code == "7")
//                {
//                    Page = Convert.ToInt32(10240 / 128);
//                }
//                if (LINES_code == "8")
//                {
//                    Page = Convert.ToInt32(16384 / 128);
//                }
//                if (LINES_code == "9")
//                {
//                    Page = Convert.ToInt32(30080 / 128);
//                }
//                Page = hex(Page)[2].zfill(2).upper();
//                msg = comm("F" + Sector + Page + "$");
//                var a = msg[28];
//                var V = new List<object>();
//                V = a.split(",");
//                V = V[:: - 1];
//                var A = new List<object>();
//                foreach (var i in Enumerable.Range(0, Convert.ToInt32(V.Count / 2)))
//                {
//                    A.append(V[i * 2 + 1] + V[i * 2]);
//                }
//                var FS = 16;
//                var BDRa = 26666.667 / Math.Pow(2, 10 - (BDR_TUPLE.index(BDR._variable.get()) + 1));
//                var acc_Array = np.zeros((Convert.ToInt32(V.Count / 2), 2));
//                var speed_Array = np.zeros((Convert.ToInt32(V.Count / 2), 2));
//                foreach (var i in Enumerable.Range(0, Convert.ToInt32(V.Count / 2)))
//                {
//                    var Integer = Convert.ToInt32(A[i], 16);
//                    if ((Integer & 1 << 16 - 1) != 0)
//                    {
//                        // convert from two's complement
//                        Integer = Integer - (1 << 16);
//                    }
//                    var gs = Integer / 32768 * FS;
//                    gs = "{:.3f}".format(gs);
//                    acc_Array[i, 0] = "{:.5f}".format(i * 1 / BDRa);
//                    acc_Array[i, 1] = gs;
//                }
//                if (clean)
//                {
//                    plot1.axes.clear();
//                }
//                if (plot)
//                {
//                    plot1.grid(true);
//                    plot1.plot(acc_Array[":", 0], acc_Array[":", 1], label: "Accel [g]");
//                    canvas.draw();
//                }
//                save_acc_Array(Ax, acc_Array);
//                var accFFT_Array = np.zeros((Convert.ToInt32(V.Count / 4), 2));
//                accFFT_Array = FFTCalc(acc_Array);
//                speed_Array = velCalc(acc_Array, Convert.ToInt32(V.Count / 2));
//                // speedFFT_Array = FFTCalc(speed_Array)
//                if (clean)
//                {
//                    plot2.axes.clear();
//                }
//                if (plot)
//                {
//                    plot2.grid(true);
//                    plot2.plot(accFFT_Array[2, 0], accFFT_Array[2, 1], label: "Accel [g]");
//                    canvas2.draw();
//                }
//                if (clean)
//                {
//                    plot3.axes.clear();
//                }
//                if (plot)
//                {
//                    plot3.grid(true);
//                    plot3.plot(speed_Array[":", 0], speed_Array[":", 1], label: "Accel [g]");
//                    canvas3.draw();
//                }
//                var acc_rms = np.sqrt(np.mean(Math.Pow(acc_Array[":", 1], 2)));
//                var vel_rms = np.sqrt(np.mean(Math.Pow(speed_Array[":", 1], 2)));
//                Status.insert("1.0", "Accel RMS = " + "{:.3f}".format(acc_rms) + " g\nSpeed RMS = " + "{:.3f}".format(vel_rms) + " mm/s");
//            }

//            public static object velCalc(object GTimeArray, object size)
//            {
//                var GTimeArrayNoG = GTimeArray.copy();
//                GTimeArrayNoG[":", 1] -= np.average(GTimeArray[":", 1]);
//                var velTimeArray = np.zeros((size, 2));
//                foreach (var i in Enumerable.Range(1, size - 1))
//                {
//                    velTimeArray[i, 0] = GTimeArray[i, 0];
//                    velTimeArray[i, 1] = velTimeArray[i - 1, 1] + GTimeArrayNoG[i - 1, 1];
//                }
//                var T = GTimeArray[1, 0] - GTimeArray[0, 0];
//                velTimeArray[":", 1::4] *= 9810 * T;
//                return velTimeArray;
//            }

//            public static object FFTCalc(object Array)
//            {
//                var N = Convert.ToInt32(Convert.ToInt32(np.prod(Array.shape)) / 2);
//                var T = Array[1, 0] - Array[0, 0];
//                var hf = np.linspace(0.0, 1.0 / (2.0 * T), Convert.ToInt32(N / 2));
//                var FFTArray = np.zeros((Convert.ToInt32(N / 2), 2), dtype: "float64");
//                var xfft = fft(Array[":", 1]);
//                FFTArray[":", 0] = hf;
//                FFTArray[":", 1] = 2.0 / N * np.abs(xfft[::int((N / 2))]);
//                return FFTArray;
//            }

//            public static object MP()
//            {
//                Status.delete("1.0", tk.END);
//                var pointer = Convert.ToInt32(comm("P$")[-4], 16) - 1;
//                var percent = pointer / 4095 * 100;
//                var msg = "Used Memory: " + pointer.ToString().zfill(4) + " / 4095 (" + "{:.2f}".format(percent) + " %)\nFree Memory: " + (4095 - pointer).ToString().zfill(4) + " / 4095 (" + "{:.2f}".format(100 - percent) + " %)\n";
//                Status.insert("1.0", msg);
//            }

//            public static object temp()
//            {
//                Status.delete("1.0", tk.END);
//                var msg = comm("t1$");
//                var Integer = Convert.ToInt32(msg, 16);
//                if ((Integer & 1 << 16 - 1) != 0)
//                {
//                    // convert from two's complement
//                    Integer = Integer - (1 << 16);
//                }
//                var T = "Temperature = " + "{:.1f} °C".format(25 + Integer / 256);
//                Status.insert("1.0", T);
//            }

//            public static object FIFO()
//            {
//                Status.delete("1.0", tk.END);
//                var msg = comm("I$");
//                Status.insert("1.0", msg);
//                Console.WriteLine(msg);
//                Status.insert("1.0", msg);
//            }

//            public static object Format()
//            {
//                Status.delete("1.0", tk.END);
//                var answer = askyesno(title: "Format Confirmation", message: "Apagar todos os dados da memória interna do sensor? \nSe decidir formatar esperar ~40 segundos com o sensor ligado (aguardar LED do sensor acender) para dar novos comandos ...");
//                if (answer)
//                {
//                    var msg = comm("format$");
//                    Status.insert("1.0", msg);
//                }
//            }

//            public static object comm(object Function)
//            {
//                // ser = serial.Serial("COM3", 115200, timeout = 6)
//                var ser = serial.Serial("/dev/ttyUSB0", 115200, timeout: 6);
//                Function = Function.encode(encoding: "UTF-8");
//                ser.write(Function);
//                var msg = "";
//                while (true)
//                {
//                    var c = ser.read().decode("utf-8");
//                    if (c == "" || c == "$")
//                    {
//                        //timeout check
//                        break;
//                    }
//                    msg += c;
//                }
//                ser.close();
//                return msg;
//            }

//            public static object root = tk.Tk();

//            public static object Button_Check = tk.Button(root, text: "Check Sensor", height: 1, width: 12, command: Checar, bg: "#d7dadd", fg: "black");

//            public static object Button_Mem = tk.Button(root, text: "Memory", height: 1, width: 12, command: MP, bg: "#d7dadd", fg: "black");

//            public static object Button_Format = tk.Button(root, text: "Format Mem.", height: 1, width: 12, command: Format, bg: "#d7dadd", fg: "black");

//            public static object Button_Acq = tk.Button(root, text: "Varredura", height: 1, width: 12, command: Sweep, bg: "#d7dadd", fg: "black", activebackground: "green");

//            public static object FREQ_TUPLE = ("Motor: 15 Hz", "Motor: 20 Hz", "Motor: 25 Hz", "Motor: 30 Hz", "Motor: 35 Hz", "Motor: 40 Hz", "Motor: 45 Hz", "Motor: 50 Hz", "Motor: 55 Hz", "Motor: 60 Hz");

//            public static object FREQ = ttk.OptionMenu(root, tk.StringVar(root), FREQ_TUPLE[0], FREQ_TUPLE);

//            public static object BDR_TUPLE = ("BDR: 52 Hz", "BDR: 104 Hz", "BDR: 208 Hz", "BDR: 417 Hz", "BDR: 833 Hz", "BDR: 1.667 Hz", "BDR: 3.333 Hz", "BDR: 6.667 Hz", "BDR: 13.333 Hz", "BDR: 26.667 Hz");

//            public static object BDR = ttk.OptionMenu(root, tk.StringVar(root), BDR_TUPLE[4], BDR_TUPLE);

//            public static object CUTOFF_TUPLE = ("Cutoff: 6.667 Hz", "Cutoff: 2.667 Hz", "Cutoff: 1.333 Hz", "Cutoff: 593 Hz", "Cutoff: 267 Hz", "Cutoff: 133 Hz", "Cutoff: 67 Hz", "Cutoff: 33 Hz");

//            public static object CUTOFF = ttk.OptionMenu(root, tk.StringVar(root), CUTOFF_TUPLE[4], CUTOFF_TUPLE);

//            public static object LPHP_TUPLE = ("HPF - High Pass Filter", "LPF - Low Pass Filter", "LPF + LPF2_Off");

//            public static object LPHP = ttk.OptionMenu(root, tk.StringVar(root), LPHP_TUPLE[1], LPHP_TUPLE);

//            public static object LINES_TUPLE = ("Lines: 512", "Lines: 1024", "Lines: 2048", "Lines: 3456", "Lines: 4096", "Lines: 6144", "Lines: 8192", "Lines: 10240", "Lines: 16384", "Lines: 30080");

//            public static object LINES = ttk.OptionMenu(root, tk.StringVar(root), LINES_TUPLE[0], LINES_TUPLE);

//            public static object AXIS_TUPLE = ("X Axis", "Y Axis", "Z Axis", "X,Y,Z Axes");

//            public static object AXIS = ttk.OptionMenu(root, tk.StringVar(root), AXIS_TUPLE[1], AXIS_TUPLE);

//            public static object PXs = tk.BooleanVar();

//            public static object PX = tk.Checkbutton(root, text: "Plot X", variable: PXs).grid(row: 16, sticky: "W");

//            public static object PYs = tk.BooleanVar();

//            public static object PY = tk.Checkbutton(root, text: "Plot Y", variable: PYs).grid(row: 17, sticky: "W");

//            public static object PZs = tk.BooleanVar();

//            public static object PZ = tk.Checkbutton(root, text: "Plot Z", variable: PZs).grid(row: 18, sticky: "W");

//            public static object Timers = tk.BooleanVar();

//            public static object Timer = tk.Checkbutton(root, text: "Timer [horas]", variable: Timers).grid(row: 20, sticky: "W");

//            public static object Timer_cycle = tk.Text(root, height: 1, width: 10);

//            public static object Button_Temp = tk.Button(root, text: "Temperature", height: 1, width: 12, command: temp, bg: "#d7dadd", fg: "black");

//            public static object y = new List<object> {
//            1,
//            1
//        };

//            public static object fig = plt.Figure(figsize: (13, 2), dpi: 100);

//            public static object plot1 = fig.add_subplot(111);

//            public static object canvas = FigureCanvasTkAgg(fig, root);

//            public static object fig2 = plt.Figure(figsize: (13, 3), dpi: 100);

//            public static object plot2 = fig2.add_subplot(111);

//            public static object canvas2 = FigureCanvasTkAgg(fig2, root);

//            public static object fig3 = plt.Figure(figsize: (13, 3), dpi: 100);

//            public static object plot3 = fig3.add_subplot(111);

//            public static object canvas3 = FigureCanvasTkAgg(fig3, root);

//            public static object Status = tk.Text(root, height: 2, width: 180);

//            public static object scrollbar = ttk.Scrollbar(root, orient: "vertical", command: Status.yview);

//            public static object cycle = 0;

//            public static object time_step = Convert.ToInt32(Timer_cycle.get(1.0, tk.END + "-1c"));

//            public static object time_step = 1;

//            public static object time_step = 1;

//            public static object time_step = 3600;

//            public static object cycle = Convert.ToInt32(time.time() / time_step);
//        }
//    }


//    //class MathCalc
//    //{


//    //}

