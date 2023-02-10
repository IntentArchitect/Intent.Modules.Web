import { Injectable } from '@nestjs/common';
import { PersonCreateDTO } from './dto/People/person-create.dto';
import { PersonDTO } from './dto/People/person.dto';
import { PersonUpdateDTO } from './dto/People/person-update.dto';
import { PersonRepository } from './../repository/person.repository';
import { IntentIgnore, IntentIgnoreBody } from './../intent/intent.decorators';
import { Person } from './../domain/entities/person.entity';
import { randomUUID } from 'crypto';

@Injectable()
export class PeopleService {

  //@IntentCanAdd()
  constructor(private personRepository: PersonRepository) { }

  @IntentIgnoreBody()
  async create(dto: PersonCreateDTO): Promise<string> {
    var newPerson = {
      name: dto.name,
    } as Person;

    await this.personRepository.save(newPerson);
    return newPerson.id;
  }

  @IntentIgnoreBody()
  async findById(id: string): Promise<PersonDTO> {
    return PersonDTO.fromPerson(await this.personRepository.findOneBy({ id: id }));
  }

  @IntentIgnoreBody()
  async findAll(): Promise<PersonDTO[]> {
    return await (await this.personRepository.find()).map(x => PersonDTO.fromPerson(x));
  }

  @IntentIgnoreBody()
  async update(id: string, dto: PersonUpdateDTO): Promise<void> {
    var existingPerson = await this.personRepository.findOneBy({
      id: id
    });
    existingPerson.name = dto.name;

    await this.personRepository.save(existingPerson);
  }

  @IntentIgnoreBody()
  async delete(id: string): Promise<void> {
    var existingPerson = await this.personRepository.findOneBy({
      id: id
    });
    await this.personRepository.remove(existingPerson);
  }
}
