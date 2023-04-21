﻿using System.Collections.Generic;

namespace SysBot.Pokemon
{
    public class ShowdownTranslatorDictionary
    {

        // 添加持有物
        public static List<string> holdItemKeywords = new List<string> { "持有", "携带" };

        // 添加异色
        public static Dictionary<string, string> shinyTypes = new Dictionary<string, string>
        {
            {"异色", "\nShiny: Yes"},
            {"闪光", "\nShiny: Yes"},
            {"星闪", "\nShiny: Star"},
            {"方闪", "\nShiny: Square"}
        };

        // 添加个体值
        public static Dictionary<string, string> ivCombos = new()
        {
            {"6V", "31 HP / 31 Atk / 31 Def / 31 SpA / 31 SpD / 31 Spe"},
            {"5V0A", "31 HP / 0 Atk / 31 Def / 31 SpA / 31 SpD / 31 Spe"},
            {"5V0攻", "31 HP / 0 Atk / 31 Def / 31 SpA / 31 SpD / 31 Spe"},
            {"5V0S", "31 HP / 31 Atk / 31 Def / 31 SpA / 31 SpD / 0 Spe"},
            {"5V0速", "31 HP / 31 Atk / 31 Def / 31 SpA / 31 SpD / 0 Spe"},
            {"4V0A0S", "31 HP / 0 Atk / 31 Def / 31 SpA / 31 SpD / 0 Spe"},
            {"4V0攻0速", "31 HP / 0 Atk / 31 Def / 31 SpA / 31 SpD / 0 Spe"}
        };

        public static Dictionary<string, string> statsDict = new()
        {
            { "生命", "HP" },
            { "攻击", "Atk" },
            { "防御", "Def" },
            { "特攻", "SpA" },
            { "特防", "SpD" },
            { "速度", "Spe" }
        };

        public static Dictionary<string, string> ribbonMarks = new()
        {
            { "最强之证", "\n.RibbonMarkMightiest=True" },
            { "未知之证", "\n.RibbonMarkRare=True" },
            { "命运之证", "\n.RibbonMarkDestiny=True" },
            { "暴雪之证", "\n.RibbonMarkBlizzard=True" },
            { "阴云之证", "\n.RibbonMarkCloudy=True" },
            { "正午之证", "\n.RibbonMarkLunchtime=True" },
            { "浓雾之证", "\n.RibbonMarkMisty=True" },
            { "降雨之证", "\n.RibbonMarkRainy=True" },
            { "沙尘之证", "\n.RibbonMarkSandstorm=True" },
            { "午夜之证", "\n.RibbonMarkSleepyTime=True" },
            { "降雪之证", "\n.RibbonMarkSnowy=True" },
            { "落雷之证", "\n.RibbonMarkStormy=True" },
            { "干燥之证", "\n.RibbonMarkDry=True" },
            { "黄昏之证", "\n.RibbonMarkDusk=True" },
            { "拂晓之证", "\n.RibbonMarkDawn=True" },
            { "上钩之证", "\n.RibbonMarkFishing=True" },
            { "咖喱之证", "\n.RibbonMarkCurry=True" },
            { "无虑之证", "\n.RibbonMarkAbsentMinded=True" },
            { "愤怒之证", "\n.RibbonMarkAngry=True" },
            { "冷静之证", "\n.RibbonMarkCalmness=True" },
            { "领袖之证", "\n.RibbonMarkCharismatic=True" },
            { "狡猾之证", "\n.RibbonMarkCrafty=True" },
            { "期待之证", "\n.RibbonMarkExcited=True" },
            { "本能之证", "\n.RibbonMarkFerocious=True" },
            { "动摇之证", "\n.RibbonMarkFlustered=True" },
            { "木讷之证", "\n.RibbonMarkHumble=True" },
            { "理性之证", "\n.RibbonMarkIntellectual=True" },
            { "热情之证", "\n.RibbonMarkIntense=True" },
            { "捡拾之证", "\n.RibbonMarkItemfinder=True" },
            { "紧张之证", "\n.RibbonMarkJittery=True" },
            { "幸福之证", "\n.RibbonMarkJoyful=True" },
            { "优雅之证", "\n.RibbonMarkKindly=True" },
            { "激动之证", "\n.RibbonMarkPeeved=True" },
            { "自信之证", "\n.RibbonMarkPrideful=True" },
            { "昂扬之证", "\n.RibbonMarkPumpedUp=True" },
            { "淘气之证", "\n.RibbonMarkRowdy=True" },
            { "凶悍之证", "\n.RibbonMarkScowling=True" },
            { "不振之证", "\n.RibbonMarkSlump=True" },
            { "微笑之证", "\n.RibbonMarkSmiley=True" },
            { "悲伤之证", "\n.RibbonMarkTeary=True" },
            { "不纯之证", "\n.RibbonMarkThorny=True" },
            { "偶遇之证", "\n.RibbonMarkUncommon=True" },
            { "自卑之证", "\n.RibbonMarkUnsure=True" },
            { "爽快之证", "\n.RibbonMarkUpbeat=True" },
            { "活力之证", "\n.RibbonMarkVigor=True" },
            { "倦怠之证", "\n.RibbonMarkZeroEnergy=True" },
            { "疏忽之证", "\n.RibbonMarkZonedOut=True" },
            { "宝主之证", "\n.RibbonMarkTitan=True" }
        };


        public static Dictionary<string, string> languages = new Dictionary<string, string>
        {
            { "异国", "Italian" },
            { "日语", "Japanese" },
            { "英语", "English" },
            { "法语", "French" },
            { "意大利语", "Italian" },
            { "德语", "German" },
            { "西班牙语", "Spanish" },
            { "韩语", "Korean" },
            { "简体中文", "ChineseS" },
            { "繁体中文", "ChineseT" }
        };

        #region 形态中文ps字典，感谢ppllouf
        public static Dictionary<string, string> formDict = new Dictionary<string, string> {
            {"阿罗拉","Alola"},
            {"初始","Original"},
            {"丰缘","Hoenn"},
            {"神奥","Sinnoh"},
            {"合众","Unova"},
            {"卡洛斯","Kalos"},
            {"就决定是你了","Partner"},
            {"搭档","Starter"},
            {"世界","World"},
            {"摇滚巨星","Rock-Star"},
            {"贵妇","Belle"},
            {"流行偶像","Pop-Star"},
            {"博士","PhD"},
            {"面罩摔角手","Libre"},
            {"换装","Cosplay"},
            {"伽勒尔","Galar"},
            {"洗翠","Hisui"},
            {"帕底亚的样子斗战种","Paldea-Combat"},
            {"帕底亚的样子火炽种","Paldea-Blaze"},
            {"帕底亚的样子水澜种","Paldea-Aqua"},
            {"刺刺耳","Spiky-eared"},
            {"帕底亚","Paldea"},
            {"B","B"},
            {"C","C"},
            {"D","D"},
            {"E","E"},
            {"F","F"},
            {"G","G"},
            {"H","H"},
            {"I","I"},
            {"J","J"},
            {"K","K"},
            {"L","L"},
            {"M","M"},
            {"N","N"},
            {"O","O"},
            {"P","P"},
            {"Q","Q"},
            {"R","R"},
            {"S","S"},
            {"T","T"},
            {"U","U"},
            {"V","V"},
            {"W","W"},
            {"X","X"},
            {"Y","Y"},
            {"Z","Z"},
            {"！","Exclamation"},
            {"？","Question"},
            {"太阳的样子","Sunny"},
            {"雨水的样子","Rainy"},
            {"雪云的样子","Snowy"},
            {"原始回归的样子","Primal"},
            {"攻击形态","Attack"},
            {"防御形态","Defense"},
            {"速度形态","Speed"},
            {"砂土蓑衣","Sandy"},
            {"垃圾蓑衣","Trash"},
            {"晴天形态","Sunshine"},
            {"东海","East"},
            {"加热","Heat"},
            {"清洗","Wash"},
            {"结冰","Frost"},
            {"旋转","Fan"},
            {"切割","Mow"},
            {"起源","Origin"},
            {"天空","Sky"},
            {"格斗","Fighting"},
            {"飞行","Flying"},
            {"毒","Poison"},
            {"地面","Ground"},
            {"岩石","Rock"},
            {"虫","Bug"},
            {"幽灵","Ghost"},
            {"钢","Steel"},
            {"火","Fire"},
            {"水","Water"},
            {"草","Grass"},
            {"电","Electric"},
            {"超能力","Psychic"},
            {"冰","Ice"},
            {"龙","Dragon"},
            {"恶","Dark"},
            {"妖精","Fairy"},
            {"蓝条纹的样子","Blue"},
            {"白条纹的样子","White"},
            {"夏天的样子","Summer"},
            {"秋天的样子","Autumn"},
            {"冬天的样子","Winter"},
            {"灵兽形态","Therian"},
            {"暗黑","White"},
            {"焰白","Black"},
            {"觉悟的样子","Resolute"},
            {"舞步形态","Pirouette"},
            {"水流卡带","Douse"},
            {"闪电卡带","Shock"},
            {"火焰卡带","Burn"},
            {"冰冻卡带","Chill"},
            {"小智版","Ash"},
            {"冰雪花纹","Icy Snow"},
            {"雪国花纹","Polar"},
            {"雪原花纹","Tundra"},
            {"大陆花纹","Continental"},
            {"庭院花纹","Garden"},
            {"高雅花纹","Elegant"},
            {"花园花纹","Meadow"},
            {"摩登花纹","Modern"},
            {"大海花纹","Marine"},
            {"群岛花纹","Archipelago"},
            {"荒野花纹","High Plains"},
            {"沙尘花纹","Sandstorm"},
            {"大河花纹","River"},
            {"骤雨花纹","Monsoon"},
            {"热带草原花纹","Savanna"},
            {"太阳花纹","Sun"},
            {"大洋花纹","Ocean"},
            {"热带雨林花纹","Jungle"},
            {"幻彩花纹","Fancy"},
            {"球球花纹","Pokeball"},
            {"蓝花","Blue"},
            {"橙花","Orange"},
            {"白花","White"},
            {"黄花","Yellow"},
            {"永恒","Eternal"},
            {"心形造型","Heart"},
            {"星形造型","Star"},
            {"菱形造型","Diamond"},
            {"淑女造型","Debutante"},
            {"贵妇造型","Matron"},
            {"绅士造型","Dandy"},
            {"女王造型","La Reine"},
            {"歌舞伎造型","Kabuki"},
            {"国王造型","Pharaoh"},
            {"小尺寸","Small"},
            {"大尺寸","Large"},
            {"特大尺寸","Super"},
            {"解放","Unbound"},
            {"啪滋啪滋风格","Pom-Pom"},
            {"呼拉呼拉风格","Pa’u"},
            {"轻盈轻盈风格","Sensu"},
            {"黑夜的样子","Midnight"},
            {"黄昏的样子","Dusk"},
            {"流星的样子","Meteor"},
            {"橙色核心","Orange"},
            {"黄色核心","Yellow"},
            {"绿色核心","Green"},
            {"浅蓝色核心","Blue"},
            {"蓝色核心","Indigo"},
            {"紫色核心","Violet"},
            {"黄昏之鬃","Dusk-Mane"},
            {"拂晓之翼","Dawn-Wings"},
            {"究极","Ultra"},
            {"５００年前的颜色","Original"},
            {"低调的样子","Low-Key"},
            {"真品","Antique"},
            {"奶香红钻","Ruby-Cream"},
            {"奶香抹茶","Matcha-Cream"},
            {"奶香薄荷","Mint-Cream"},
            {"奶香柠檬","Lemon-Cream"},
            {"奶香雪盐","Salted-Cream"},
            {"红钻综合","Ruby-Swirl"},
            {"焦糖综合","Caramel-Swirl"},
            {"三色综合","Rainbow-Swirl"},
            {"剑之王","Crowned"},
            {"盾之王","Crowned"},
            {"无极巨化","Eternamax"},
            {"连击流","Rapid-Strike"},
            {"阿爸","Dada"},
            {"骑白马的样子","Ice"},
            {"骑黑马的样子","Shadow"},
            {"四只家庭","Four"},
            {"蓝羽毛","Blue"},
            {"黄羽毛","Yellow"},
            {"白羽毛","White"},
            {"下垂姿势","Droopy"},
            {"平挺姿势","Stretchy"},
            {"三节形态","Three-Segment"},
        };
        #endregion

    }
}
