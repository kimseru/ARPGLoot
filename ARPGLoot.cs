using Terraria.ModLoader;

namespace ARPGLoot
{
    class ARPGLoot : Mod
    {
        public ARPGLoot()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadSounds = true
            };
        }
    }
}
