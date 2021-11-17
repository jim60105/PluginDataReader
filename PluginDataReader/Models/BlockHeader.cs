using MessagePack;

namespace PluginDataReader.Models
{
    // Token: 0x0200011C RID: 284
    [MessagePackObject(true)]
    public class BlockHeader
    {
        // Token: 0x17000223 RID: 547
        // (get) Token: 0x06000A86 RID: 2694 RVA: 0x0004A28A File Offset: 0x0004848A
        // (set) Token: 0x06000A87 RID: 2695 RVA: 0x0004A292 File Offset: 0x00048492
#pragma warning disable IDE1006 // 命名樣式
        public List<BlockHeader.Info> lstInfo { get; set; }

        // Token: 0x06000A88 RID: 2696 RVA: 0x0004A29B File Offset: 0x0004849B
        public BlockHeader()
        {
            this.lstInfo = new List<BlockHeader.Info>();
        }

        // Token: 0x06000A89 RID: 2697 RVA: 0x0004A2B0 File Offset: 0x000484B0
        public BlockHeader.Info SearchInfo(string name)
        {
#pragma warning disable CS8603 // 可能有 Null 參考傳回。
            return this.lstInfo.Find((BlockHeader.Info n) => n.name == name);
#pragma warning restore CS8603 // 可能有 Null 參考傳回。
        }

        // Token: 0x02000B67 RID: 2919
        [MessagePackObject(true)]
        public class Info
        {
            // Token: 0x17001564 RID: 5476
            // (get) Token: 0x0600658E RID: 25998 RVA: 0x00239881 File Offset: 0x00237A81
            // (set) Token: 0x0600658F RID: 25999 RVA: 0x00239889 File Offset: 0x00237A89
            public string name { get; set; }

            // Token: 0x17001565 RID: 5477
            // (get) Token: 0x06006590 RID: 26000 RVA: 0x00239892 File Offset: 0x00237A92
            // (set) Token: 0x06006591 RID: 26001 RVA: 0x0023989A File Offset: 0x00237A9A
            public string version { get; set; }

            // Token: 0x17001566 RID: 5478
            // (get) Token: 0x06006592 RID: 26002 RVA: 0x002398A3 File Offset: 0x00237AA3
            // (set) Token: 0x06006593 RID: 26003 RVA: 0x002398AB File Offset: 0x00237AAB
            public long pos { get; set; }

            // Token: 0x17001567 RID: 5479
            // (get) Token: 0x06006594 RID: 26004 RVA: 0x002398B4 File Offset: 0x00237AB4
            // (set) Token: 0x06006595 RID: 26005 RVA: 0x002398BC File Offset: 0x00237ABC
            public long size { get; set; }

            // Token: 0x06006596 RID: 26006 RVA: 0x002398C5 File Offset: 0x00237AC5
            public Info()
            {
                this.name = "";
                this.version = "";
                this.pos = 0L;
                this.size = 0L;
            }
        }
#pragma warning restore IDE1006 // 命名樣式
    }
}
