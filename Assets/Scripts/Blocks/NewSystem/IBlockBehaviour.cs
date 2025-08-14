public interface IBlockBehaviour
{
    void StartBehaviour();
    void StopBehaviour();
    void OnBounced(HelmetInstance _helmetInstance);
    void OnHeadbutt(HelmetInstance _helmetInstance);
}