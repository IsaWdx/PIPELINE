using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int CPU_Cycle;
        OpenFileDialog Dialog1;
        OpenFileDialog Dialog2;
        bool stepback;
        pipeline P;
        public Form1()
        {
            InitializeComponent();
            CPU_Cycle = 0;
            stepback = false;
            timer1.Interval = 1000;
            //timer1.Start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.Azure;
            //this.BackgroundImage = Image.FromFile("C:\\Users\\lenovo\\Pictures\\桌面\\381297.png");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dialog1 = new OpenFileDialog();
            Dialog2 = new OpenFileDialog();
            pipeline P = new pipeline();

        }
        protected void button2_Click(object sender, EventArgs e)
        {
        }
        protected void button1_Click(object sender, EventArgs e)
        {

            if (Dialog1.ShowDialog() == DialogResult.OK)
            {
                this.TextPath.SelectedText = Dialog1.FileName;
                string path = this.TextPath.Text;
                StreamReader sr = new StreamReader(path, Encoding.Default);
                P = new pipeline();
                pipeline.FetchAll(sr);
                sr.Close();
            }
        }

        private void Output_Click(object sender, EventArgs e)
        {
            this.Dialog2.Filter = "|*.*";
            if (Dialog2.ShowDialog() == DialogResult.OK)
            {
                this.outputPath.SelectedText = Dialog2.FileName;
                string path = this.outputPath.Text;
                pipeline.fs = new FileStream(path, FileMode.Create);
                if(pipeline.rw!=null)pipeline.rw.Close();
                pipeline.rw = new StreamWriter(pipeline.fs);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (pipeline.end != 1)
            {
                stepin(P, CPU_Cycle);
                CPU_Cycle++;
            }
            else

                MessageBox.Show("End with IHALT or failed");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        bool stepin(pipeline P, int count)
        {
            if (pipeline.Instruction_Memory.Length < 1)
            {
                MessageBox.Show("Load a .yo!'");
                return false;
            }
            P.WriteMain();
            P.MemoryMain();
            P.ExecuteMain();
            P.DecodeMain();
            P.FetchMain();
            if (stepback == false)
            {
                if (pipeline.rw != null)
                {
                    output(count);
                }
                set_label(count);
            }
            P.ControlLogic();
            P.clock_up();
            return true;
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void label64_Click(object sender, EventArgs e)
        {

        }

        private void label63_Click(object sender, EventArgs e)
        {
        }
        private string convert_icode(int icode)
        {
            if (icode == pipeline.IHALT)
                return "I_HALT";
            if (icode == pipeline.INOP)
                return "I_NOP";
            if(icode==pipeline.IJXX)
                return "I_JXX";
            if(icode==pipeline.IIRMOVL)
                return "I_IRMOVL";
            if(icode==pipeline.IMRMOVL)
                return "I_MRMOVL";
            if(icode==pipeline.IRRMOVL)
                return "I_RRMOVL";
            if(icode==pipeline.IRMMOVL)
                return "I_RMMOVL";
            if(icode==pipeline.IOPL)
                return "I_OPL";
            if(icode==pipeline.ICALL)
                return "I_CALL";
            if(icode==pipeline.IRET)
                return "I_RET";
            if(icode==pipeline.IPUSHL)
                return "I_PUSHL";
            if(icode==pipeline.IPOPL)
                return "I_POPL";
            return "wrong";
        }
        private string convert_stat(int stat)
        {
            if (stat == pipeline.SAOK)
                return "SAOK";
            if (stat == pipeline.SINS)
                return "SINS";
            if (stat == pipeline.SADR)
                return "SADR";
            if (stat == pipeline.SBUB)
                return "SBUB";
            if (stat == pipeline.SHLT)
                return "SHLT";
            else return "Wrong";
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label79_Click(object sender, EventArgs e)
        {

        }

        private void labeledx_Click(object sender, EventArgs e)
        {

        }

        private void label55_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            if (this.hz1.Checked) timer1.Interval = 1000;
            if (this.Hz2.Checked) timer1.Interval = 500;
            if (this.hz5.Checked) timer1.Interval = 200;
            if (this.Hz10.Checked) timer1.Interval = 100;
            if (this.Hz50.Checked) timer1.Interval = 20;
            timer1.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            reset();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pipeline.end != 1)
            {
                stepin(P, CPU_Cycle);
                CPU_Cycle++;
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("End with IHALT or failed");
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            back_one_step();

        }
        private void set_label2(int count)
        {
            this.label15.Text = convert_stat(P.Stat);
            this.label14.Text = count.ToString();
            this.label38.Text = convert_stat(P.D_stat);
            this.label45.Text = convert_stat(P.E_stat);
            this.label23.Text = convert_stat(P.M_stat);
            this.label29.Text = convert_stat(P.W_stat);

            //F
            this.label16.Text = "0x" + P.F_predPC.ToString("x");
            //D
            this.label44.Text = "0x" + P.D_icode.ToString("x");
            this.label43.Text = "0x" + P.D_ifun.ToString("x");
            this.label42.Text = "0x" + P.D_rA.ToString("x");
            this.label41.Text = "0x" + P.D_rB.ToString("x");
            this.label39.Text = "0x" + P.D_valC.ToString("x8");
            this.label40.Text = "0x" + P.D_valP.ToString("x8");
            //E
            this.label50.Text = "0x" + P.E_icode.ToString("x");
            this.label49.Text = "0x" + P.E_ifun.ToString("x");
            this.label47.Text = "0x" + P.E_valC.ToString("x8");
            this.label46.Text = "0x" + P.E_valA.ToString("x8");
            this.label48.Text = "0x" + P.E_valB.ToString("x8");
            this.label56.Text = "0x" + P.E_dstE.ToString("x");
            this.label34.Text = "0x" + P.E_dstM.ToString("x");
            this.label54.Text = "0x" + P.E_srcA.ToString("x");
            this.label53.Text = "0x" + P.E_srcB.ToString("x");
            //M
            this.label57.Text = "0x" + P.M_icode.ToString("x");
            if (P.M_Cnd)
                this.label28.Text = "true";
            else
                this.label28.Text = "false";
            this.label25.Text = "0x" + P.M_valE.ToString("x8");
            this.label24.Text = "0x" + P.M_valA.ToString("x8");
            this.label27.Text = "0x" + P.M_dstE.ToString("x");
            this.label26.Text = "0x" + P.M_dstM.ToString("x");
            //W
            this.label31.Text = "0x" + P.W_valE.ToString("x8");
            this.label30.Text = "0x" + P.W_valM.ToString("x8");
            this.label33.Text = "0x" + P.W_dstE.ToString("x");
            this.label32.Text = "0x" + P.W_dstM.ToString("x");
            this.label65.Text = "0x" + P.W_icode.ToString("x");
            //Register
            this.labeleax.Text = "0x" + pipeline.Register[0].ToString("x8");
            this.labelecx.Text = "0x" + pipeline.Register[1].ToString("x8");
            this.labeledx.Text = "0x" + pipeline.Register[2].ToString("x8");
            this.labelebx.Text = "0x" + pipeline.Register[3].ToString("x8");
            this.labelesp.Text = "0x" + pipeline.Register[4].ToString("x8");
            this.labelebp.Text = "0x" + pipeline.Register[5].ToString("x8");
            this.labelesi.Text = "0x" + pipeline.Register[6].ToString("x8");
            this.labeledi.Text = "0x" + pipeline.Register[7].ToString("x8");
        }
        private void set_label(int count)
        {
            this.label15.Text = convert_stat(P.Stat);
            this.label14.Text = count.ToString();
            this.label38.Text = convert_stat(P.D_stat);
            this.label45.Text = convert_stat(P.E_stat);
            this.label23.Text = convert_stat(P.M_stat);
            this.label29.Text = convert_stat(P.W_stat);

            //F
            this.label16.Text = "0x" + P.F_predPC.ToString("x");
            //D
            this.label44.Text = convert_icode(P.D_icode);
            this.label43.Text = "0x" + P.D_ifun.ToString("x");
            this.label42.Text = "0x" + P.D_rA.ToString("x");
            this.label41.Text = "0x" + P.D_rB.ToString("x");
            this.label39.Text = "0x" + P.D_valC.ToString("x8");
            this.label40.Text = "0x" + P.D_valP.ToString("x8");
            //E
            this.label50.Text = convert_icode(P.E_icode);
            this.label49.Text = "0x" + P.E_ifun.ToString("x");
            this.label47.Text = "0x" + P.E_valC.ToString("x8");
            this.label46.Text = "0x" + P.E_valA.ToString("x8");
            this.label48.Text = "0x" + P.E_valB.ToString("x8");
            this.label56.Text = "0x" + P.E_dstE.ToString("x");
            this.label34.Text = "0x" + P.E_dstM.ToString("x");
            this.label54.Text = "0x" + P.E_srcA.ToString("x");
            this.label53.Text = "0x" + P.E_srcB.ToString("x");
            //M
            this.label57.Text = convert_icode(P.M_icode);
            if (P.M_Cnd)
                this.label28.Text = "true";
            else
                this.label28.Text = "false";
            this.label25.Text = "0x" + P.M_valE.ToString("x8");
            this.label24.Text = "0x" + P.M_valA.ToString("x8");
            this.label27.Text = "0x" + P.M_dstE.ToString("x");
            this.label26.Text = "0x" + P.M_dstM.ToString("x");
            //W
            this.label31.Text = "0x" + P.W_valE.ToString("x8");
            this.label30.Text = "0x" + P.W_valM.ToString("x8");
            this.label33.Text = "0x" + P.W_dstE.ToString("x");
            this.label32.Text = "0x" + P.W_dstM.ToString("x");
            this.label65.Text = convert_icode(P.W_icode);
            //Register
            String a0 = labeleax.Text, a1 = labelecx.Text, a2 = labeledx.Text, a3 = labelebx.Text,
                a4 = labelesp.Text, a5 = labelebp.Text, a6 = labelesi.Text, a7 = labeledi.Text;

            this.labeleax.Text = "0x" + pipeline.Register[0].ToString("x8");
            this.labelecx.Text = "0x" + pipeline.Register[1].ToString("x8");
            this.labeledx.Text = "0x" + pipeline.Register[2].ToString("x8");
            this.labelebx.Text = "0x" + pipeline.Register[3].ToString("x8");
            this.labelesp.Text = "0x" + pipeline.Register[4].ToString("x8");
            this.labelebp.Text = "0x" + pipeline.Register[5].ToString("x8");
            this.labelesi.Text = "0x" + pipeline.Register[6].ToString("x8");
            this.labeledi.Text = "0x" + pipeline.Register[7].ToString("x8");
            if (a0 != this.labeleax.Text) labeleax.BackColor = Color.Green;
            else labeleax.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a1 != this.labelecx.Text) labelecx.BackColor = Color.Green;
            else labelecx.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a2 != this.labeledx.Text) labeledx.BackColor = Color.Green;
            else labeledx.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a3 != this.labelebx.Text) labelebx.BackColor = Color.Green;
            else labelebx.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a4 != this.labelesp.Text) labelesp.BackColor = Color.Green;
            else labelesp.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a5 != this.labelebp.Text) labelebp.BackColor = Color.Green;
            else labelebp.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a6 != this.labelesi.Text) labelesi.BackColor = Color.Green;
            else labelesi.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            if (a7 != this.labeledi.Text) labeledi.BackColor = Color.Green;
            else labeledi.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
        }
        private void reset(){            
            timer1.Stop();
            CPU_Cycle = 0;
            P = new pipeline();
            stepback = false;
            set_label(0);
        }
        private void back_one_step()
        {
            if (CPU_Cycle == 0)
            {
                MessageBox.Show("Already CPU_Cycle 0!");
                return;
            }
            stepback = true;
            int cnt = CPU_Cycle - 2;
            reset();
            while (CPU_Cycle < cnt)
            {
                stepin(P, CPU_Cycle);
                CPU_Cycle++;
            }
            stepback = false;
            stepin(P, CPU_Cycle);
            CPU_Cycle++;     
        }
        private void output(int count)
        {
            pipeline.rw.WriteLine("Cycle_{0}", count);
            pipeline.rw.WriteLine("--------------------");


            pipeline.rw.WriteLine("FETCH:");

            pipeline.rw.WriteLine("\tF_predPC\t= 0x{0}", P.F_predPC.ToString("x"));





            pipeline.rw.WriteLine("DECODE:");
            pipeline.rw.WriteLine("\tD_icode\t\t= 0x{0:x}", P.D_icode.ToString("x"));

            pipeline.rw.WriteLine("\tD_ifun\t\t= 0x{0:x}", P.D_ifun.ToString("x"));

            pipeline.rw.WriteLine("\tD_rA\t\t= 0x{0:x}", P.D_rA.ToString("x"));

            pipeline.rw.WriteLine("\tD_rB\t\t= 0x{0:x}", P.D_rB.ToString("x"));

            pipeline.rw.WriteLine("\tD_valC\t\t= 0x{0:x8}", P.D_valC);

            pipeline.rw.WriteLine("\tD_valP\t\t= 0x{0:x8}", P.D_valP);


            //E
            pipeline.rw.WriteLine("EXECUTE:");
            pipeline.rw.WriteLine("\tE_icode\t\t= 0x{0:x}\t", P.E_icode);
            pipeline.rw.WriteLine("\tE_ifun\t\t= 0x{0:x}\t", P.E_ifun);

            pipeline.rw.WriteLine("\tE_valC\t\t= 0x{0:x8}\t", P.E_valC);
            pipeline.rw.WriteLine("\tE_valA\t\t= 0x{0:x8}\t", P.E_valA);
            pipeline.rw.WriteLine("\tE_valB\t\t= 0x{0:x8}\t", P.E_valB);


            pipeline.rw.WriteLine("\tE_dstE\t\t= 0x{0:x}\t", P.E_dstE);
            pipeline.rw.WriteLine("\tE_dstM\t\t= 0x{0:x}\t", P.E_dstM);
            pipeline.rw.WriteLine("\tE_srcA\t\t= 0x{0:x}\t", P.E_srcA);
            pipeline.rw.WriteLine("\tE_srcB\t\t= 0x{0:x}\t", P.E_srcB);
            //M

            pipeline.rw.WriteLine("MEMORY:");
            pipeline.rw.WriteLine("\tM_icode\t\t= 0x{0:x}\t", P.M_icode);
            pipeline.rw.WriteLine("\tM_Bch\t\t= {0:x}\t", P.M_Cnd);
            pipeline.rw.WriteLine("\tM_valE\t\t= 0x{0:x8}\t", P.M_valE);
            pipeline.rw.WriteLine("\tM_valA\t\t= 0x{0:x8}\t", P.M_valA);
            pipeline.rw.WriteLine("\tM_dstE\t\t= 0x{0:x}\t", P.M_dstE);
            pipeline.rw.WriteLine("\tM_dstM\t\t= 0x{0:x}\t", P.M_dstM);
            //W

            pipeline.rw.WriteLine("WRITE BACK:");
            pipeline.rw.WriteLine("\tW_valE\t\t= 0x{0:x8}\t", P.W_valE);
            pipeline.rw.WriteLine("\tW_valM\t\t= 0x{0:x8}\t", P.W_valM);
            pipeline.rw.WriteLine("\tW_dstE\t\t= 0x{0:x}\t", P.W_dstE);
            pipeline.rw.WriteLine("\tW_dstM\t\t= 0x{0:x}\t", P.W_dstM);

            //pipeline.rw.WriteLine("REGISTER");
            //pipeline.rw.WriteLine("\t%eax\t\t= 0x{0:x8}\t",pipeline.Register[0]);
            //pipeline.rw.WriteLine("\t%ecx\t\t= 0x{0:x8}\t", pipeline.Register[1]);
            //pipeline.rw.WriteLine("\t%edx\t\t= 0x{0:x8}\t", pipeline.Register[2]);
            //pipeline.rw.WriteLine("\t%ebx\t\t= 0x{0:x8}\t", pipeline.Register[3]);
            //pipeline.rw.WriteLine("\t%esp\t\t= 0x{0:x8}\t", pipeline.Register[4]);

            //pipeline.rw.WriteLine("\t%ebp\t\t= 0x{0:x8}\t", pipeline.Register[5]);
            //pipeline.rw.WriteLine("\t%esi\t\t= 0x{0:x8}\t", pipeline.Register[6]);
            //pipeline.rw.WriteLine("\t%edi\t\t= 0x{0:x8}\t", pipeline.Register[7]);

            pipeline.rw.Flush();

        }
    }
}