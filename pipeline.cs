using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class pipeline
{
    public pipeline()
    {

        ZF = SF = OF = false;
        //这里需要初始化一些东西
        Memory = new byte[16097280];
        Register = new int[8];
        for (int i = 0; i < 8; i++)
            Register[i] = 0;
        M_stat = m_stat = W_stat = f_stat = E_stat = D_stat = SBUB;
        Stat = SAOK;
        f_pc =d_valA = d_valB = 0;
        W_dstE = W_dstM = E_dstE = M_dstE = e_dstE = d_srcA = d_srcB = E_srcA = E_dstM =
        E_srcB = M_dstM = w_dstM = w_dstE = D_rA = D_rB = d_dstE = d_dstM = RNONE;
        aluA = aluB = alufun = 0;
        D_icode = D_valP = 0;
        d_rvalA = d_rvalB = 0;
        e_valA = w_valE = w_valM = 0;
        f_predPC = F_predPC = imem_icode = imem_ifun = f_icode = f_ifun = f_valC = f_valP = 0;
         M_icode = M_ifun = M_valA = M_valE = 0;
        m_valM = W_icode = W_valE = W_valM = 0;
        E_icode = E_ifun = E_valC = E_valA = E_valB = e_valE = 0;
        F_stall = F_bubble = D_stall = D_bubble = E_stall = E_bubble = M_stall = M_bubble = W_stall = W_bubble = false;
        set_cc = true;
        need_regids = need_valC = mem_read = mem_write = imem_error = instr_valid = e_Cnd = M_Cnd = dmem_error = false;
        mem_addr = 0;
        end = 0;
        D_ifun = D_valC = 0;
    }
    public void clock_up()
    {
        int fD_stat, fD_icode, fD_ifun, fD_rA, fD_rB, fD_valC, fD_valP;
        int fE_stat, fE_icode, fE_ifun, fE_valC, fE_valA, fE_valB, fE_dstE, fE_dstM, fE_srcA, fE_srcB;
        int fM_stat, fM_icode, fM_ifun, fM_valE, fM_valA, fM_dstE, fM_dstM;
        bool fM_Cnd;
        int fW_stat, fW_icode, fW_valE, fW_valM, fW_dstE, fW_dstM;
        if (w_dstE <= 7)
        {
            Register[w_dstE] = w_valE;
            //Console.WriteLine("Register{0} after write{1}", w_dstE,pipeline.Register[w_dstE]);
        }

        if (w_dstM <= 7)
        {
            Register[w_dstM] = w_valM;
            //Console.WriteLine("afterwrite Register4{0}", pipeline.Register[4]);
        }

        fD_stat = D_stat;
        fD_icode = D_icode;
        fD_ifun = D_ifun;
        fD_rA = D_rA;
        fD_rB = D_rB;
        fD_valC = D_valC;
        fD_valP = D_valP;

        fE_stat = E_stat;
        fE_icode = E_icode;
        fE_ifun = E_ifun;
        fE_valC = E_valC;
        fE_valA = E_valA;
        fE_valB = E_valB;
        fE_dstE = E_dstE;
        fE_dstM = E_dstM;
        fE_srcA = E_srcA;
        fE_srcB = E_srcB;

        fM_stat = M_stat;
        fM_icode = M_icode;
        fM_ifun = M_ifun;
        fM_Cnd = M_Cnd;
        fM_valE = M_valE;
        fM_valA = M_valA;
        fM_dstE = M_dstE;
        fM_dstM = M_dstM;

        fW_stat = W_stat;
        fW_icode = W_icode;
        fW_valE = W_valE;
        fW_valM = W_valM;
        fW_dstE = W_dstE;
        fW_dstM = W_dstM;
        if (F_stall)
        {
            ;
        }
        else
        {
            F_predPC = f_predPC;
        }
        if (D_stall) { ;}
        else if (D_bubble) { set_D_bubble(); D_stat = SBUB; }
        else
        {
            D_stat = f_stat;
            D_icode = f_icode;
            D_ifun = f_ifun;
            D_rA = f_rA;
            D_rB = f_rB;
            D_valC = f_valC;
            D_valP = f_valP;
        }
        if (E_stall) { ;}
        else if (E_bubble) { set_E_bubble(); E_stat = SBUB; }
        else
        {
            E_stat = fD_stat;
            E_icode = fD_icode;
            E_ifun = fD_ifun;
            E_valC = fD_valC;
            E_valA = d_valA;
            E_valB = d_valB;
            E_dstE = d_dstE;
            E_dstM = d_dstM;
            E_srcA = d_srcA;
            E_srcB = d_srcB;
        }
        if (M_stall) { ;}
        else if (M_bubble) { set_M_bubble(); M_stat = SBUB; }
        else
        {
            M_stat = fE_stat;
            M_icode = fE_icode;
            M_ifun = fE_ifun;
            M_Cnd = e_Cnd;
            M_valE = e_valE;
            M_valA = e_valA;
            M_dstE = e_dstE;
            M_dstM = fE_dstM;
        }
        if (W_stall) { ;}
        else if (W_bubble) { set_W_bubble(); W_stat = SBUB; }
        else
        {
            W_stat = m_stat;
            W_icode = fM_icode;
            W_valE = fM_valE;
            W_valM = m_valM;
            W_dstE = fM_dstE;
            W_dstM = fM_dstM;
        }
        Stat = fW_stat;
        if (Stat == SHLT || Stat == SINS || Stat == SADR) end = 1;
    }

    public static FileStream fs;
    public static StreamWriter rw;
    public static FileStream sw;
    public static int end;
    public static String Instruction_Memory;
    public static byte[] Memory;
    public static int[] Register;
    public static bool ZF, SF, OF;

    //指令宏定义
    public const int IHALT = 0;
    public const int INOP = 1;
    public const int IRRMOVL = 2;
    public const int IIRMOVL = 3;
    public const int IRMMOVL = 4;
    public const int IMRMOVL = 5;
    public const int IOPL = 6;
    public const int IJXX = 7;
    public const int ICALL = 8;
    public const int IRET = 9;
    public const int IPUSHL = 0xa;
    public const int IPOPL = 0xb;

    public const int FNONE = 0;

    public const int RESP = 4;
    public const int RNONE = 8;

    public const int ALUADD = 0;

    public const int SAOK = 1;
    public const int SADR = 2;
    public const int SINS = 3;
    public const int SHLT = 4;
    public static int SBUB = 5;
    //跳转类型宏定义
    public const int IJMP = 0;
    public const int IJLE = 1;
    public const int IJL = 2;
    public const int IJE = 3;
    public const int IJNE = 4;
    public const int IJGE = 5;
    public const int IJG = 6;

    //寄存器宏定义
    public const int REAX = 0;
    public const int RECX = 1;
    public const int REDX = 2;
    public const int REBX = 3;
    public const int REBP = 5;
    public const int RESI = 6;
    public const int REDI = 7;
    //supplimentary signal
    //signal  F
    public int f_pc;
    public int f_stat;
    public bool need_regids;
    public bool need_valC;
    public int f_rA;
    public int f_rB;
    public int f_predPC;
    //signal D
    public int d_valA;
    public int d_valB;
    public int d_dstE;
    public int d_dstM;
    public int D_stat;
    public int D_ifun;
    public int D_valC;
    //signal E
    public int E_stat;
    public int aluA;
    public int aluB;
    public int alufun;
    public bool set_cc;
    public int e_valA;
    //signal M
    public int mem_addr;
    public bool mem_read;
    public bool mem_write;
    //signal W
    public int w_dstE;
    public int w_valE;
    public int w_dstM;
    public int w_valM;
    public int Stat;

    //Pipeline Register F 
    public int F_predPC;// Predicted value of PC

    //Intermediate Values in Fetch Stage 
    public int imem_icode;// icode field from instruction memory 
    public int imem_ifun;// ifun field from instruction memory 
    public int f_icode;// (Possibly modified) instruction code 
    public int f_ifun;// Fetched instruction function 
    public int f_valC;// Constant data of fetched instruction 
    public int f_valP;// Address of following instruction 
    public bool imem_error;// Error signal from instruction memory 
    public bool instr_valid;// Is fetched instruction valid?


    //Pipeline Register D;
    public int D_icode;// Instruction code 
    public int D_rA;// rA field from instruction 
    public int D_rB;// rB field from instruction 
    public int D_valP;// Incremented PC

    //Intermediate Values in Decode Stage //

    public int d_srcA;// srcA from decoded instruction 
    public int d_srcB;// srcB from decoded instruction 
    public int d_rvalA;// valA read from register file 
    public int d_rvalB;// valB read from register file

    //Pipeline Register E;
    public int E_icode;// Instruction code 
    public int E_ifun;// Instruction function 
    public int E_valC;// Constant data 
    public int E_srcA;// Source A register ID 
    public int E_valA;// Source A value 
    public int E_srcB;// Source B register ID 
    public int E_valB;// Source B value 
    public int E_dstE;// Destination E register ID 
    public int E_dstM;// Destination M register ID

    //Intermediate Values in Execute Stage // 
    public int e_valE;// valE generated by ALU 
    public bool e_Cnd;// Does condition hold?
    public int e_dstE;// dstE (possibly modified to be RNONE)

    //Pipeline Register M // 
    public int M_stat;// Instruction status 
    public int M_icode;// Instruction code 
    public int M_ifun;// Instruction function 
    public int M_valA;// Source A value 
    public int M_dstE;// Destination E register ID 
    public int M_valE;// ALU E value 
    public int M_dstM;// Destination M register ID 
    public bool M_Cnd;// Condition flag 
    public bool dmem_error;// Error signal from instruction memory

    //Intermediate Values in Memory Stage//
    public int m_valM;// valM generated by memory 
    public int m_stat;// stat (possibly modified to be SADR)

    //Pipeline Register W;
    public int W_stat;// Instruction status 
    public int W_icode;// Instruction code 
    public int W_dstE;// Destination E register ID 
    public int W_valE;// ALU E value 
    public int W_dstM;// Destination M register ID 
    public int W_valM;// Memory M value

    //later added 
    public bool F_stall;
    public bool F_bubble;
    public bool D_stall;
    public bool D_bubble;
    public bool E_stall;
    public bool E_bubble;
    public bool M_stall;
    public bool M_bubble;
    public bool W_stall;
    public bool W_bubble;
    //
    // Control Signal Definitions. // 
    //
    public static int FetchAll(StreamReader sr)
    {
        String line, code;
        int pc_current = 0;
        while ((line = sr.ReadLine()) != null)//to be changed to do one at once
        {

            if (line.Length < 1) continue;
            String[] a = line.Split('|');
            code = a[0];

            String[] b = code.Split(':');
            if (b.Length < 2) continue;
            if (b[1][2] == ' ') continue;
            // 将所有指令存起来 顺序执行/跳转
            String code_address = b[0].Substring(2), instruction = b[1].Substring(1);
            //Console.WriteLine("{0},{1},{2}", instruction, code_address, instruction);
            //Start the fetch process
            instruction = instruction.Trim().TrimEnd("union all".ToCharArray());
            code_address.Trim().TrimStart();
            code_address.Trim().TrimEnd();
            code_address = code_address.Substring(2);
            for (int i = pc_current; i < Convert.ToInt32(code_address, 16); i++)
            { Instruction_Memory += "00"; pc_current++; }
            pc_current += instruction.Length / 2;
            Instruction_Memory += instruction;
        }
        //Console.WriteLine("{0}", Instruction_Memory);
        sr.Close();
        return 1;
    }

    public bool ControlLogic()
    {

        // Should I stall or inject a bubble into Pipeline Register F? 
        // At most one of these can be true. 
        F_bubble = false;
        F_stall = (     // Conditions for a load/use hazard 
        E_icode == IMRMOVL || E_icode == IPOPL) &&
        (E_dstM == d_srcA || E_dstM == d_srcB) ||
            // Stalling at fetch while ret passes through pipeline 
        (IRET == D_icode || IRET == E_icode || IRET == M_icode);

        // Should I stall or inject a bubble into Pipeline Register D? 
        // At most one of these can be true. 
        D_stall =// Conditions for a load/use hazard 
      (E_icode == IMRMOVL || E_icode == IPOPL) &&
      (E_dstM == d_srcA || E_dstM == d_srcB);
        //rw.WriteLine("D_stall is {0}", D_stall);

        D_bubble =
            // Mispredicted branch 
        (E_icode == IJXX && !e_Cnd) ||
            // Stalling at fetch while ret passes through pipeline 
            // but not condition for a load/use hazard 
        !((E_icode == IMRMOVL || E_icode == IPOPL) && (E_dstM == d_srcA || E_dstM == d_srcB)) &&
        (IRET == D_icode || IRET == E_icode || IRET == M_icode);

        // Should I stall or inject a bubble into Pipeline Register E? 
        // At most one of these can be true. 
        E_stall = false;
        //rw.WriteLine("{0},{1},{2},{3},{4},D_bubble is {5}",E_icode,e_Cnd,E_dstM,d_srcA,d_srcB,D_bubble);
        E_bubble =
            // Mispredicted branch 
        (E_icode == IJXX && !e_Cnd) ||
            // Conditions for a load/use hazard 
        (E_icode == IMRMOVL || E_icode == IPOPL) &&
       (E_dstM == d_srcA || E_dstM == d_srcB);

        //rw.WriteLine("{0},{1},{2},{3},{4},E_bubble is {5}", E_icode, e_Cnd, E_dstM, d_srcA, d_srcB, E_bubble);

        // Should I stall or inject a bubble into Pipeline Register M?
        // At most one of these can be true. 
        M_stall = false;
        // Start injecting bubbles as soon as exception passes through memory stage 
        M_bubble = (m_stat == SADR || m_stat == SINS || m_stat == SHLT) || (W_stat == SADR || W_stat == SINS || W_stat == SHLT);
        // Should I stall or inject a bubble into Pipeline Register W? 
        W_stall = W_stat == SADR || W_stat == SINS || W_stat == SHLT;
        W_bubble = false;
        return true;
    }

    public bool set_D_bubble()
    {
        D_icode = INOP;
        D_rA = RNONE;
        D_rB = RNONE;
        D_stat = SBUB;
        D_ifun = 0;
        return true;
    }
    public bool set_E_bubble()
    {
        E_stat = SBUB;
        E_icode = INOP;
        E_dstM = RNONE;
        E_dstE = RNONE;
        E_srcA = RNONE;
        E_srcB = RNONE;
        E_ifun = 0;
        return true;
    }
    public bool set_M_bubble()
    {
        M_stat = SBUB;
        M_icode = INOP;
        return true;
    }
    public bool set_W_bubble()
    {
        M_stat = SBUB;
        M_icode = INOP;
        M_ifun = 0;
        return true;
    }

    //Fetch Stage Operation
    public bool FetchMain()
    {

        f_pc = F_f_pc();
        F_PC_memory_increase_INS();
        ////Console.WriteLine(f_pc);
        f_icode = F_f_icode();
        f_ifun = F_f_ifun();
        instr_valid = F_instr_valid();
        f_stat = F_f_stat();
        need_regids = F_need_regids();
        need_valC = F_need_valC();
        f_predPC = F_f_predPC();
        return true;
    }
    public bool F_PC_memory_increase_INS()
    {
        if (f_pc * 2 + 1 >= Instruction_Memory.Length)
        {
            return false;
        }
        //Console.WriteLine("{0},{1}", f_pc, Instruction_Memory.Length);
        imem_icode = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2, 1), 16);
        //Console.WriteLine("icode:{0},ifun:{1}", imem_icode, imem_ifun);
        imem_ifun = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2 + 1, 1), 16);
        if (imem_icode == IHALT || imem_icode == INOP || imem_icode == IRET)
        {
            f_valP = f_pc + 1;
            pipeline.rw.WriteLine("\tmem_icode\t= 0x{0}", imem_icode);
            pipeline.rw.WriteLine("\t0x{0}0x{1}", f_valP, f_pc);
            f_rA = RNONE;
            f_rB = RNONE;
        }
        else if (imem_icode == IRRMOVL && imem_ifun < 7 || imem_icode == IOPL && imem_ifun < 4 || imem_icode == IPUSHL && imem_ifun == 0 || imem_icode == IPOPL && imem_ifun == 0)
        {
            f_valP = f_pc + 2;
            f_rA = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2 + 2, 1), 16);
            f_rB = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2 + 3, 1), 16);
            if (f_rA == 8)
                f_rA = RNONE;
            if (f_rB == 8)
                f_rB = RNONE;
        }
        else if (imem_icode == IJXX || imem_icode == ICALL)
        {
            f_rA = RNONE;
            f_rB = RNONE;
            f_valP = f_pc + 5;
            f_valC = little_endian(Instruction_Memory.Substring(f_pc * 2 + 2, 8));
        }
        else if ((imem_icode == IIRMOVL || imem_icode == IRRMOVL || imem_icode == IMRMOVL) && imem_ifun == 0)
        {
            f_valP = f_pc + 6;
            f_rA = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2 + 2, 1), 16);
            f_rB = Convert.ToInt32(Instruction_Memory.Substring(f_pc * 2 + 3, 1), 16);
            if (f_rA == 8)
                f_rA = RNONE;
            if (f_rB == 8)
                f_rB = RNONE;
            f_valC = little_endian(Instruction_Memory.Substring(f_pc * 2 + 4, 8));
        }
        else imem_error = true;
        return true;
    }
    int little_endian(String a)
    {
        char c0 = a[6], c1 = a[7], c2 = a[4], c3 = a[5], c4 = a[2], c5 = a[3], c6 = a[0], c7 = a[1];
        String x = "";
        x = x + c0 + c1 + c2 + c3 + c4 + c5 + c6 + c7;
        return Convert.ToInt32(x, 16);
    }
    int F_f_pc()
    {
        // Mispredicted branch. Fetch at incremented PC 
        if (M_icode == IJXX && !M_Cnd) return M_valA;
        // Completion of RET instruction. 
        else if (W_icode == IRET) return W_valM;
        // Default: Use predicted value of PC 
        return F_predPC;
    }
    // Determine icode of fetched instruction 
    int F_f_icode()
    {
        if (imem_error) return INOP;
        return imem_icode;
    }

    // Determine ifun 
    int F_f_ifun()
    {
        if (imem_error) return FNONE;
        return imem_ifun;
    }

    // Is instruction valid? 
    bool F_instr_valid()
    {
        if (f_icode == INOP || f_icode == IHALT || f_icode == IRRMOVL || f_icode == IIRMOVL || f_icode == IRMMOVL || f_icode == IMRMOVL || f_icode == IOPL
           || f_icode == IJXX || f_icode == ICALL || f_icode == IRET || f_icode == IPUSHL || f_icode == IPOPL)
            return true;
        else return false;
    }
    // Determine status code for fetched instruction 
    int F_f_stat()
    {
        if (imem_error) return SADR;
        if (!instr_valid) return SINS;
        if (f_icode == IHALT) return SHLT;
        return SAOK;
    }
    // Does fetched instruction require a regid byte? 
    bool F_need_regids()
    {
        return f_icode == IRRMOVL || f_icode == IOPL || f_icode == IPUSHL || f_icode == IPOPL || f_icode == IIRMOVL || f_icode == IRMMOVL
            || f_icode == IMRMOVL;
    }
    // Does fetched instruction require a constant word? 
    bool F_need_valC()
    {
        return f_icode == IIRMOVL || f_icode == IRMMOVL || f_icode == IMRMOVL || f_icode == IJXX || f_icode == ICALL;
    }
    // Predict next value of PC 
    int F_f_predPC()
    {
        if (f_icode == IJXX || f_icode == ICALL) return f_valC;
        return f_valP;
    }
    //Decode Stage Operation
    public bool DecodeMain()
    {
        d_srcA = D_d_srcA();
        d_srcB = D_d_srcB();
        d_dstE = D_d_dstE();
        d_dstM = D_d_dstM();
        d_valA = D_d_valA();
        d_valB = D_d_valB();
        return true;
    }

    //// What register should be used as the A source? 
    int D_d_srcA()
    {
        if (D_icode == IRRMOVL || D_icode == IRMMOVL || D_icode == IOPL || D_icode == IPUSHL)
        {

            return D_rA;
        }
        if (D_icode == IPOPL || D_icode == IRET) return RESP;
        return RNONE; // Don’t need register 
    }

    //// What register should be used as the B source? 
    int D_d_srcB()
    {
        if (D_icode == IOPL || D_icode == IRMMOVL || D_icode == IMRMOVL) return D_rB;
        if (D_icode == IPUSHL || D_icode == IPOPL || D_icode == ICALL || D_icode == IRET) return RESP;
        return RNONE; // Don’t need register 
    }

    //// What register should be used as the E destination? 
    int D_d_dstE()
    {

        if (D_icode == IRRMOVL || D_icode == IIRMOVL || D_icode == IOPL) return D_rB;
        if (D_icode == IPUSHL || D_icode == IPOPL || D_icode == ICALL || D_icode == IRET) return RESP;
        return RNONE; // Don’t write any register 
    }

    //// What register should be used as the M destination? 
    int D_d_dstM()
    {
        if (D_icode == IMRMOVL || D_icode == IPOPL) return D_rA;
        return RNONE; // Don’t write any register 
    }

    //// What should be the A value? 
    //// Forward into decode stage for valA 
    int D_d_valA()
    {
        if (D_icode == ICALL || D_icode == IJXX)
        {
            //rw.WriteLine("D_valP"); 
            return D_valP;
        } // Use incremented PC 
        if (d_srcA == e_dstE)
        {
            //rw.WriteLine("e_valE");
            return e_valE;
        } // Forward valE from execute 
        if (d_srcA == M_dstM)
        {
            //rw.WriteLine("{0}m_valM{1},{2}", d_srcA, M_dstM,m_valM); 
            return m_valM;
        } // Forward valM from memory 
        if (d_srcA == M_dstE)
        {
            //rw.WriteLine("M_valE");
            return M_valE;
        }// Forward valE from memory 
        if (d_srcA == W_dstM)
        {
            //  rw.WriteLine("W_valM");
            return W_valM;
        } // Forward valM from write back 
        if (d_srcA == W_dstE)
        {
            //    rw.WriteLine("W_valE"); 
            return W_valE;
        } // Forward valE from write back 
        d_rvalA = Register[d_srcA];
        return d_rvalA; // Use value read from register file 
    }

    int D_d_valB()
    {
        if (d_srcB == e_dstE) return e_valE; // Forward valE from execute 
        if (d_srcB == M_dstM) return m_valM; // Forward valM from memory 
        if (d_srcB == M_dstE) return M_valE; // Forward valE from memory 
        if (d_srcB == W_dstM) return W_valM; // Forward valM from write back 
        if (d_srcB == W_dstE) return W_valE; // Forward valE from write back 
        d_rvalB = Register[d_srcB];
        return d_rvalB; // Use value read from register file 
    }

    //Execute Stage Operation
    public bool ExecuteMain()
    {

        alufun = E_alufun();
        e_dstE = E_e_dstE();
        e_valE = E_e_valE();
        e_valA = E_e_valA();

        return true;
    }
    // Select input A to ALU 
    public int E_aluA()
    {
        if (E_icode == IRRMOVL || E_icode == IOPL) return E_valA;
        if (E_icode == IIRMOVL || E_icode == IRMMOVL || E_icode == IMRMOVL) return E_valC;
        if (E_icode == ICALL || E_icode == IPUSHL) return -4;
        if (E_icode == IRET || E_icode == IPOPL) return 4;
        // Other instructions don’t need ALU 
        return 0;
    }

    // Select input B to ALU 
    public int E_aluB()
    {
        if (E_icode == IRMMOVL || E_icode == IMRMOVL || E_icode == IOPL || E_icode == ICALL || E_icode == IPUSHL || E_icode == IRET || E_icode == IPOPL)
            return E_valB;
        if (E_icode == IRRMOVL || E_icode == IIRMOVL) return 0;
        // Other instructions don’t need ALU 
        return 0;
    }
    // Set the ALU function 
    public int E_alufun()
    {
        if (E_icode == IOPL) return E_ifun;
        return ALUADD;
    }

    // Should the condition codes be updated? 
    public bool E_gset_cc()
    {
        return E_icode == IOPL &&      // State chanE_ges only during normal operation 
             !(m_stat == SADR || m_stat == SINS || m_stat == SHLT) && !(W_stat == SADR || W_stat == SINS || W_stat == SHLT);

    }
    // Generate valA in execute stage 
    public int E_e_valA()
    {
        //rw.WriteLine("E_valA={0}", E_valA);
        return E_valA; // Pass valA throuE_gh stage
    }

    // Set dstE to RNONE in event of not-taken conditional move 
    public int E_e_dstE()
    {

        if (E_icode == IRRMOVL && !e_Cnd) return RNONE;

        return E_dstE;
    }
    public int E_e_valE()
    {
        aluA = E_aluA();
        aluB = E_aluB();
        int tmp;
        switch (alufun)
        {
            case 1: tmp = aluB - aluA; break;
            case 2: tmp = aluB & aluA; break;
            case 3: tmp = (aluB ^ aluA); break;
            default: tmp = aluB + aluA; break;
        }
        set_cc = E_gset_cc();
        if (tmp == 0 && set_cc)
        {
            ZF = true; OF = SF = false;
        }
        if (tmp < 0 && set_cc)
        {
            SF = true; ZF = OF = false;
        }
        if ((aluA < 0 == aluB < 0) && (tmp < 0 != aluB < 0) && set_cc)
        {
            OF = true; ZF = SF = false;
        }
        if (E_icode == IJXX || E_icode == IRRMOVL)
            switch (E_ifun)
            {
                case IJMP:
                    {
                        e_Cnd = true;
                        break;
                    }
                case IJLE:
                    {
                        e_Cnd = ((SF ^ OF) | ZF);
                        break;
                    }
                case IJL:
                    {
                        e_Cnd = SF ^ OF;
                        break;
                    }
                case IJE:
                    {
                        e_Cnd = ZF;
                        break;
                    }
                case IJNE:
                    {
                        e_Cnd = !ZF;
                        break;
                    }
                case IJGE:
                    {
                        e_Cnd = !(SF ^ OF);
                        break;
                    }
                case IJG:
                    {
                        e_Cnd = !(SF ^ OF) & !ZF;
                        break;
                    }
            }
        else e_Cnd = true;
        return tmp;

    }
    //Memory Stage Operation
    public bool MemoryMain()
    {
        mem_addr = M_mem_addr();
        mem_read = M_mem_read();
        mem_write = M_mem_write();
        Memory_Write();
        Memory_Read();
        m_stat = M_m_stat();

        return true;
    }
    void Memory_Write()
    {
        if (mem_write)
        {

            Memory[mem_addr] = (byte)(M_valA);
            Memory[mem_addr + 1] = (byte)(M_valA >> 8);
            Memory[mem_addr + 2] = (byte)(M_valA >> 16);
            Memory[mem_addr + 3] = (byte)(M_valA >> 24);
        }
    }
    void Memory_Read()
    {
        int a1, a2, a3, a4;
        if (mem_read)
        {
            if (mem_addr < Instruction_Memory.Length / 2 - 4)
            {
                a1 = Convert.ToInt32(Instruction_Memory.Substring(mem_addr * 2, 2), 16);
                a2 = Convert.ToInt32(Instruction_Memory.Substring(mem_addr * 2 + 2, 2), 16);
                a3 = Convert.ToInt32(Instruction_Memory.Substring(mem_addr * 2 + 4, 2), 16);
                a4 = Convert.ToInt32(Instruction_Memory.Substring(mem_addr * 2 + 6, 2), 16);
            }
            else
            {
                a1 = Memory[mem_addr];
                a2 = Memory[mem_addr + 1];
                a3 = Memory[mem_addr + 2];
                a4 = Memory[mem_addr + 3];
            }
            m_valM = a1 | (a2 << 8) | (a3 << 16) | (a4 << 24);
        }
    }
    // Select memory address 
    int M_mem_addr()
    {
        if (M_icode == IRMMOVL || M_icode == IPUSHL || M_icode == ICALL || M_icode == IMRMOVL)
        {
            return M_valE;
        }
        if (M_icode == IPOPL || M_icode == IRET) return M_valA;
        // Other instructions don’t need address 
        return M_valA;//can be wrong
    }

    // Set read control signal 
    bool M_mem_read()
    {
        return (M_icode == IMRMOVL || M_icode == IPOPL || M_icode == IRET);
    }

    // Set write control signal 
    bool M_mem_write()
    {
        return (M_icode == IRMMOVL || M_icode == IPUSHL || M_icode == ICALL);
    }

    ///* $begin pipe-m_stat-hcl */ 
    // Update the status 
    int M_m_stat()
    {
        if (dmem_error) return SADR;
        return M_stat;
    }
    //Write Stage Operation
    public bool WriteMain()
    {
        w_dstE = W_w_dstE();
        w_valE = W_w_valE();
        w_dstM = W_w_dstM();
        w_valM = W_w_valM();
        Stat = W_Stat();
        return true;
    }

    int W_w_dstE()
    {
        return W_dstE;
    }
    // Set E port value 
    int W_w_valE()
    {
        return W_valE;
    }
    // Set M port reW_gister ID 
    int W_w_dstM()
    {
        return W_dstM;
    }
    // Set M port value 
    int W_w_valM()
    {
        return W_valM;
    }
    // Update processor status 
    int W_Stat()
    {
        if (W_stat == SBUB) return SAOK;
        return W_stat;
    }
}
