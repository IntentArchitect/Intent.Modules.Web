import { Repository } from 'typeorm';
import { CustomRepository } from './../typeorm/typeorm-ex.decorator';
import { Person } from './../domain/entities/person.entity';
import { IntentMerge } from './../intent/intent.decorators';

@IntentMerge()
@CustomRepository(Person)
export class PersonRepository extends Repository<Person> {
}