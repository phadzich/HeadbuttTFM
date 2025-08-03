public interface IBlockEffect
{
    void OnBounced(HelmetInstance _helmetInstance);
    void OnHeadbutt(HelmetInstance _helmetInstance);
    void Activate();
}