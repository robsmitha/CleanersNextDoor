using Application.Common.Mappings;
using Application.LineItems;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<CreateAddressModel>
    {
        public int CustomerID { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
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
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, CreateAddressModel>
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
        public async Task<CreateAddressModel> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
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
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            var address = new CustomerAddress
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
                Note = request.Note
            };
            _context.CustomerAddresses.Add(address);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CreateAddressModel>(address);
        }
    }
}
