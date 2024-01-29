using Biblioteka.Context;
using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Biblioteka.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : Controller
    {
        private string _filePath = "../Biblioteka/wwwroot/rentQueue.json"; // ścieżka do pliku rentqueue.json       
        private readonly BibContext _context;

        public QueueController(BibContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetJsonContent()
        {
            try
            {
                string jsonData = System.IO.File.ReadAllText(_filePath);
                return Content(jsonData, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest($"Błąd odczytu pliku JSON: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddToQueue([FromBody] Models.Queue queue)
        {
            try
            {
                var queues = LoadQueues();

                if (!queues.ContainsKey(queue.BookId))
                {
                    queues[queue.BookId] = new List<PseudoQueue>();                
                    PseudoQueue newItem = new(queue.UserId, queue.Quantity);
                    queues[queue.BookId].Add(newItem);
                }
                else
                {
                    bool found = false;
                    foreach(var item in queues[queue.BookId])
                    {
                        if(item.UserId == queue.UserId)
                        {
                            item.Quantity++;
                            found = true;
                            break;
                        }
                    }
                    if (found == false) 
                    {
                        PseudoQueue newItem = new(queue.UserId, queue.Quantity);
                        queues[queue.BookId].Add(newItem);
                    }
                }
              
                SaveQueues(queues);
                return Json(new { success = true, message = "Product added to the queue successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateQueue(int id, [FromBody] Models.Queue updatedQueue)
        {
            try
            {
                var queues = LoadQueues();
                if (queues.ContainsKey(id))
                {
                    var originalQueue = queues[id];

                    if (updatedQueue.Quantity != 0)
                    {
                        bool found = false;
                        foreach (var item in originalQueue)
                        {
                            if (item.UserId == updatedQueue.UserId)
                            {
                                item.Quantity = updatedQueue.Quantity;
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            return Json(new { success = false, message = "Queue not found." });
                        }
                    }
                        
                    SaveQueues(queues);

                    return Json(new { success = true, message = "Queue updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Queue not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFromQueue(int id, [FromBody] Models.Queue deleteQueue)
        {
            try
            {
                var queues = LoadQueues();

                if (queues.ContainsKey(id))
                {
                    var queueList = queues[id];

                    if(deleteQueue!=null)
                        queueList.Remove(queueList.Where(item => item.UserId == deleteQueue.UserId).FirstOrDefault());                    

                    if (queueList.Count == 0)
                    {
                        queues.Remove(id);
                    }

                    SaveQueues(queues);

                    return Json(new { success = true, message = "Queue updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Queue not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        private Dictionary<int, List<PseudoQueue>> LoadQueues()
        {
            try
            {
                var json = System.IO.File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<Dictionary<int, List<PseudoQueue>>>(json) ?? new Dictionary<int, List<PseudoQueue>>();
            }
            catch
            {
                return new Dictionary<int, List<PseudoQueue>>();
            }
        }

        private void SaveQueues(Dictionary<int, List<PseudoQueue>> queues)
        {
            var json = JsonConvert.SerializeObject(queues);
            System.IO.File.WriteAllText(_filePath, json);
        }       
    }
}
