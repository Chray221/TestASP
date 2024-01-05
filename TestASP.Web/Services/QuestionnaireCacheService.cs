using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Caching.Memory;
using TestASP.Web.Areas.Admin.Models.Questionnaire;
using TestASP.Web.Services;

namespace TestASP.Web.Services;

public class QuestionnaireCacheService
// : CachedDataService
{
    public readonly MyMemoryCache _cache ;
    private Dictionary<string,object> Questionnaires;

    public QuestionnaireCacheService(
        MyMemoryCache cache) 
    {
        _cache = cache;
        GetAllQuestionnaires();
    }    

    private Dictionary<string,object> GetAllQuestionnaires()
    {
        Questionnaires ??= _cache.Get<Dictionary<string,object>>(nameof(Questionnaires)) ?? new Dictionary<string, object>();
        
        return Questionnaires;
    }

    public void Save(string key, AdminQuestionnaireQuestionsViewModel newQuestionnaire)
    {        
        SaveQuestionnaire(key,newQuestionnaire);
    }

    public AdminQuestionnaireQuestionsViewModel Get(string key)
    {        
        return GetQuestionnaire<AdminQuestionnaireQuestionsViewModel>(key);
    }

    public Dictionary<string,AdminQuestionnaireQuestionsViewModel> GetQuestionnaires()
    {
        return Questionnaires.Where( q => q.Value is AdminQuestionnaireQuestionsViewModel).ToDictionary( q => q.Key, q => (AdminQuestionnaireQuestionsViewModel)q.Value);
    }
    

    private void Save<T>(string key, T value)
    {
        SaveQuestionnaire(key,value);
    }

    private T Get<T>(string key)
    {
        return GetQuestionnaire<T>(key);
    }

    private Dictionary<string,T> GetQuestionnaires<T>()
    {
        return Questionnaires.Where( q => q.Value is T).ToDictionary( q => q.Key, q => (T)q.Value);
    }

    private void SaveQuestionnaire<T>(string key, T value)
    {
        Questionnaires[key] = value;
        _cache.Set(nameof(Questionnaires),Questionnaires);
    }

    public T GetQuestionnaire<T>(string key)
    {
        return (T)Questionnaires.FirstOrDefault( q => q.Value is T && q.Key == key).Value;
    }

    // public QuestionnaireCacheService(
    //     ProtectedLocalStorage localStorage) 
    //     : base(localStorage)
    // {
    // } 

    // public async Task SaveAsync<T>(string key, T value)
    // {
    //     await SaveQuestionnaireAsync(key,value);
    // }

    // public Task<T> GetAsync<T>(string key)
    // {
    //     return GetQuestionnaire<T>(key);
    // }

    // public async Task<Dictionary<string,T>> GetQuestionnaires<T>()
    // {
    //     var questions = await GetQuestionnaires();
    //     return questions.Where( q => q.Value is T).ToDictionary( q => q.Key, q => (T)q.Value);
    // }

    // private async Task<Dictionary<string,object>> GetQuestionnaires()
    // {
    //     if(Questionnaires == null)
    //     {
            
    //         var questionnairesResult = await _localStorage.GetAsync<Dictionary<string,object>>(nameof(Questionnaires));
    //         if(questionnairesResult.Success)
    //         {
    //             Questionnaires = questionnairesResult.Value;
    //         }
    //     }
    //     Questionnaires = Questionnaires ?? new Dictionary<string,object>();

    //     return Questionnaires;
    // }

    // private async Task SaveQuestionnaireAsync<T>(string key, T value)
    // {
    //     Dictionary<string,object> questionnaire = await GetQuestionnaires();
    //     questionnaire[key] = value;
    //     await _localStorage.SetAsync(nameof(Questionnaires),questionnaire);
    // }

    // public async Task<T> GetQuestionnaire<T>(string key)
    // {
    //     var questions = await GetQuestionnaires();
    //     return (T)questions.FirstOrDefault( q => q.Value is T && q.Key == key).Value;
    // }
}
