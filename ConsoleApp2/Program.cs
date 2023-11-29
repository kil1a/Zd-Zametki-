
class Note
{
    public string Title { get; set; }
    public string Content { get; set; }
}

static List<Note> notes = new List<Note>();









// Обработка различных HTTP-методов
switch (request.HttpMethod)
{
    case "GET":
        // Получение списка всех заметок
        if (request.Url.AbsolutePath == "/notes")
        {
            string responseData = Newtonsoft.Json.JsonConvert.SerializeObject(notes);
            byte[] buffer = Encoding.UTF8.GetBytes(responseData);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        // Получение конкретной заметки по заголовку
        else if (request.Url.Segments.Length == 3 && request.Url.Segments[1] == "notes/")
        {
            string title = request.Url.Segments[2];
            Note note = notes.Find(n => n.Title == title);
            if (note != null)
            {
                string responseData = Newtonsoft.Json.JsonConvert.SerializeObject(note);
                byte[] buffer = Encoding.UTF8.GetBytes(responseData);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        // Неизвестный URL
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        break;

    case "POST":
        // Добавление новой заметки
        if (request.Url.AbsolutePath == "/notes")
        {
            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string requestData = reader.ReadToEnd();
                Note note = Newtonsoft.Json.JsonConvert.DeserializeObject<Note>(requestData);
                notes.Add(note);
                response.StatusCode = (int)HttpStatusCode.Created;
            }
        }
        // Неизвестный URL
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        break;

    case "PUT":
        // Обновление заметки по заголовку
        if (request.Url.Segments.Length == 3 && request.Url.Segments[1] == "notes/")
        {
            string title = request.Url.Segments[2];
            Note noteToUpdate = notes.Find(n => n.Title == title);
            if (noteToUpdate != null)
            {
                using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string requestData = reader.ReadToEnd();
                    Note updatedNote = Newtonsoft.Json.JsonConvert.DeserializeObject<Note>(requestData);
                    noteToUpdate.Content = updatedNote.Content;
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        // Неизвестный URL
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        break;

    case "DELETE":
        // Удаление заметки по заголовку
        if (request.Url.Segments.Length == 3 && request.Url.Segments[1] == "notes/")
        {
            string title = request.Url.Segments[2];
            Note noteToDelete = notes.Find(n => n.Title == title);
            if (noteToDelete != null)
            {
                notes.Remove(noteToDelete);
                response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        // Неизвестный URL
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        break;

    default:
        // Неподдерживаемый HTTP-метод
        response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
        break;
}