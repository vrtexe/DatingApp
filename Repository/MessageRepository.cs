using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Dtos;
using Model.Entities;
using Model.Params;
using Repository.Extensions;
using Repository.interfaces;

namespace Repository;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Connection> GetConnection(string connectionId)
    {
        return await _context.Connections.FindAsync(connectionId);
    }

    public async Task<Group> GetGroupForConnection(string connectionId)
    {
        return await _context.Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(x => x.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    public async Task<Message> GetMessage(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<Group> GetMessageGroup(string groupName)
    {
        return await _context.Groups.Include(g => g.Connections).FirstOrDefaultAsync(g => g.Name == groupName);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _context.Messages
                .OrderByDescending(msg => msg.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .AsQueryable()
                .FilterByContainer(messageParams);

        return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Messages
            .Where
            (
                m =>
                (m.Recipient.UserName == currentUsername
                && m.Sender.UserName == recipientUsername
                && !m.RecipientDeleted)
                || (m.Recipient.UserName == recipientUsername
                && m.Sender.UserName == currentUsername
                && !m.SenderDeleted)
            )
            .OrderByDescending(m => m.MessageSent)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Any())
        {
            unreadMessages.ForEach(msg => msg.DateRead = DateTime.UtcNow);
        }

        return messages;
    }

    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }
}