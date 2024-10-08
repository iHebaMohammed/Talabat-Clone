﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasketDTO basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(basket);
            var UpdatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            return Ok(UpdatedOrCreatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            //return Ok(result);
        }
    }
}
