using ActivityTracer.Models;

namespace ActivityTracer.Data
{
	public interface IAppActivityRepository
	{
		void Create(AppActivity activity);
		void DeleteFromId(string id);
		IEnumerable<AppActivity> Read();
		AppActivity? ReadFromId(string id);
		void Update(AppActivity activity);
	}
}