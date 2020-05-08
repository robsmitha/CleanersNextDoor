using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<bool>
    {
        public int CustomerID { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }
        public CreateAddressCommand(int customerId, CreateAddressModel model)
        {
            CustomerID = customerId;
            Street1 = model.Street1;
            Street2 = model.Street2;
            City = model.City;
            StateAbbreviation = model.StateAbbreviation;
            Zip = model.Zip;
            Latitude = model.Latitude;
            Longitude = model.Longitude;
            Name = model.Name;
            Note = model.Note;
            IsDefault = model.IsDefault;
        }
    }
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, bool>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            if (request.IsDefault)
            {
                //set other customer addresses to not default
                var entities = _context.CustomerAddresses
                    .Where(a => a.CustomerID == request.CustomerID && a.IsDefault)
                    .ToList();
                if(entities.Count != 0)
                {
                    entities.ForEach(e => e.IsDefault = false);
                    _context.CustomerAddresses.UpdateRange(entities);
                }
            }

            //TODO: pass correspondence type id from ui 
            //customer can configure pickup/delivery for each workflow
            var correspondenceTypes = _context.CorrespondenceTypes
                .Where(c => c.CustomerConfigures);

            var addresses = correspondenceTypes.Select(c => new CustomerAddress
            {
                CustomerID = request.CustomerID,
                Street1 = request.Street1,
                Street2 = request.Street2,
                City = request.City,
                StateAbbreviation = request.StateAbbreviation,
                Zip = request.Zip,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsDefault = request.IsDefault,
                Name = request.Name,
                Note = request.Note,
                CorrespondenceTypeID = c.ID
            });
            _context.CustomerAddresses.AddRange(addresses);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
